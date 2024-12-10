using UnityEngine;
using Vuplex.WebView;

public class VisionOSWebViewManager : MonoBehaviour
{
    private WebViewPrefab _webViewPrefab;
    
    void Start()
    {
        // Create WebViewPrefab instance with dimensions (0.6 width, 0.4 height)
        _webViewPrefab = WebViewPrefab.Instantiate(0.6f, 0.4f);
        
        // Add the material replacer component
        var materialReplacer = _webViewPrefab.gameObject.AddComponent<WebViewMaterialReplacer>();
        materialReplacer.webViewPrefab = _webViewPrefab;
        
        // Position the webview as needed
        _webViewPrefab.transform.parent = transform;
        _webViewPrefab.transform.localPosition = new Vector3(0, 0, 0.5f);
        _webViewPrefab.transform.LookAt(transform);
        
        // Load URL after initialization
        InitializeWebView();
    }
    
    async void InitializeWebView()
    {
        await _webViewPrefab.WaitUntilInitialized();
        _webViewPrefab.WebView.LoadUrl("http://localhost:3000/operation"); // Replace with your desired URL
    }
}