#include "stdafx.h"
#include <Commdlg.h>
#include <algorithm>

using namespace winrt;
using namespace winrt::Windows::Foundation;
using namespace winrt::Windows::Storage::Streams;
using namespace winrt::Windows::Web::UI;
using namespace winrt::Windows::Web::UI::Interop;
using namespace Microsoft::WRL;
using namespace Microsoft::WRL::Wrappers;
using namespace winrt::Windows::System;

void CheckFailure(_In_ HRESULT hr)
{
    if (FAILED(hr))
    {
        WCHAR message[512] = L"";
        StringCchPrintf(message, ARRAYSIZE(message), L"Error: 0x%x", hr);
        MessageBoxW(nullptr, message, nullptr, MB_OK);
        ExitProcess(-1);
    }
}


Rect HwndWindowRectToBoundsRect(_In_ HWND hwnd)
{
	RECT windowRect = { 0 };
	GetWindowRect(hwnd, &windowRect);

	Rect bounds =
	{
		0,
		0,
		static_cast<float>(windowRect.right - windowRect.left),
		static_cast<float>(windowRect.bottom - windowRect.top)
	};

	return bounds;
}

winrt::Windows::Web::UI::Interop::WebViewControlProcessOptions App::CreatePrivateNetworkProcessOptions()
{
	WebViewControlProcessOptions processOptions = WebViewControlProcessOptions();
	processOptions.PrivateNetworkClientServerCapability(winrt::Windows::Web::UI::Interop::WebViewControlProcessCapabilityState::Enabled);
	return processOptions;
}




std::wstring App::GetUserAgent()
{
    std::wstring html = std::wstring(L"<html><head><script>function GetUserAgent(){return navigator.userAgent;}</script></head></html>");
    Event operationCompleted(CreateEvent(nullptr, true, false, nullptr));
    HANDLE h = operationCompleted.Get();
    m_webViewControl.NavigateToString(hstring(html.c_str()));
    ::Sleep(100);
    IAsyncOperation<hstring> asyncscript(nullptr);
    asyncscript = m_webViewControl.InvokeScriptAsync(hstring(L"GetUserAgent"), nullptr);
    asyncscript.Completed([this, h](IAsyncOperation<hstring> const& sender, AsyncStatus args)
    {
        m_user_agent = sender.GetResults().c_str();
        SetEvent(h);
    });
    WaitForSingleObject(operationCompleted.Get(), INFINITE);
    return m_user_agent;
}

void App::NavigateWithHeader(const winrt::Windows::Foundation::Uri & uri)
{
    HttpMethod httpmethod = HttpMethod(hstring(L"GET"));
    HttpRequestMessage requestMessage = HttpRequestMessage(httpmethod, uri);
    requestMessage.Headers().Append(param::hstring(L"User-Agent"), param::hstring(m_user_agent.c_str()));
    m_webViewControl.NavigateWithHttpRequestMessage(requestMessage);
    AddNavigationStarting();
}

void App::AddNavigationStarting()
{
    m_nav_start_token = m_webViewControl.NavigationStarting([this](IWebViewControl const& sender, winrt::Windows::Web::UI::IWebViewControlNavigationStartingEventArgs args)
    {
        if (this->m_user_agent.size() > 0)
        {
            args.Cancel(true);
            m_webViewControl.NavigationStarting(this->m_nav_start_token); // remove handler
            NavigateWithHeader(args.Uri());
        }
    });
}

void App::InitializeWin32WebView()
{
    if (!m_processOptions)
        m_processOptions = winrt::Windows::Web::UI::Interop::WebViewControlProcessOptions();
    if (!m_process)
        m_process = winrt::Windows::Web::UI::Interop::WebViewControlProcess();

    auto asyncwebview = m_process.CreateWebViewControlAsync((int64_t)m_hostWindow, HwndWindowRectToBoundsRect(m_hostWindow));
    asyncwebview.Completed([this](IAsyncOperation<WebViewControl> const& sender, AsyncStatus args)
    {
        this->m_webViewControl = sender.GetResults();

        m_user_agent = GetUserAgent();
        m_user_agent.append(L" WebViewSampleCppWinrt/1.00");

        this->m_webViewControl.ContentLoading([this](IWebViewControl const& sender, winrt::Windows::Web::UI::IWebViewControlContentLoadingEventArgs args)
        {
            if (args.Uri())
                SetWindowText(m_addressbarWindow, args.Uri().DisplayUri().c_str());
        });

        this->m_webViewControl.NewWindowRequested([this](IWebViewControl const& sender, winrt::Windows::Web::UI::IWebViewControlNewWindowRequestedEventArgs args)
        {
            std::wstring uri = args.Uri().DisplayUri().c_str();
            std::transform(uri.begin(), uri.end(), uri.begin(), towlower);
			if (uri.find(L".pdf") != std::wstring::npos)
				Launcher::LaunchUriAsync(args.Uri());
            else
                m_webViewControl.Navigate(args.Uri());
        });

        AddNavigationStarting();

        this->m_webViewControl.Navigate(Uri(hstring(L"https://www.bing.com/")));
    });
}

void App::SetScale(_In_ double scale)
{
	winrt::Windows::Web::UI::Interop::IWebViewControlSite webViewControlSite = (winrt::Windows::Web::UI::Interop::IWebViewControlSite) m_webViewControl;
    webViewControlSite.Scale(scale);
}

void App::ResizeWebView()
{
	Rect bounds = HwndWindowRectToBoundsRect(m_hostWindow);
    winrt::Windows::Web::UI::Interop::IWebViewControlSite webViewControlSite = (winrt::Windows::Web::UI::Interop::IWebViewControlSite) m_webViewControl;
	webViewControlSite.Bounds(bounds);
}

void App::SaveScreenshot()
{
    InMemoryRandomAccessStream stream = InMemoryRandomAccessStream();
    auto capturePreviewToStreamAsyncAction = m_webViewControl.CapturePreviewToStreamAsync(stream);
    capturePreviewToStreamAsyncAction.Completed([this, stream](IAsyncAction const& sender, AsyncStatus args)
    {
		OPENFILENAME openFileName = {};
		openFileName.lStructSize = sizeof(openFileName);
		openFileName.hwndOwner = m_hostWindow;
		openFileName.hInstance = m_hInst;
		WCHAR fileName[MAX_PATH] = L"screenshot.png";
		openFileName.lpstrFile = fileName;
		openFileName.nMaxFile = ARRAYSIZE(fileName);

		if (GetSaveFileName(&openFileName))
		{
            auto datareader = DataReader(stream.GetInputStreamAt(0));
            UINT64 size = stream.Size();
            auto loadAsyncOperation = datareader.LoadAsync(size);
            loadAsyncOperation.Completed([this, datareader, size, fileName](IAsyncOperation<UINT32> const& sender, AsyncStatus args)
            {
				BYTE* bytes = new BYTE[size];
				for (UINT32 position = 0; position < size; ++position)
                    bytes[position] = datareader.ReadByte();

				HANDLE file = CreateFile(fileName, GENERIC_WRITE, 0, nullptr, CREATE_NEW, FILE_ATTRIBUTE_NORMAL, nullptr);
                if (file != INVALID_HANDLE_VALUE)
                {
                    WriteFile(file, bytes, size, nullptr, nullptr);
                    CloseHandle(file);
                    file = INVALID_HANDLE_VALUE;
                }
            });
		}
	});
}

void App::ToggleVisibility()
{
    winrt::Windows::Web::UI::Interop::IWebViewControlSite webViewControlSite = (winrt::Windows::Web::UI::Interop::IWebViewControlSite) m_webViewControl;
    boolean visible = webViewControlSite.IsVisible();
    webViewControlSite.IsVisible(!visible);
}

void App::NavigateToUri(_In_ LPCWSTR uriAsString)
{
    winrt::Windows::Foundation::Uri uri(uriAsString);
    m_webViewControl.Navigate(uri);
}
