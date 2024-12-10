using System.Threading.Tasks;
using UnityEngine;
using Vuplex.WebView;

/// <summary>
/// Demonstrates how to use the VisionOSWebView.CreateInWindow() API to open a webview
/// in a native visionOS (SwiftUI) window.
/// </summary>
public class VisionOSRealityKitWebViewExample : MonoBehaviour {

    private IWebView webView;

    async void Start() {
        Debug.Log("Starting VisionOS RealityKit WebView Example...");

        try {
            #if UNITY_VISIONOS && !UNITY_EDITOR
                Debug.Log("Initializing VisionOS WebView...");
                webView = await VisionOSWebView.CreateInWindow();
                Debug.Log("VisionOS WebView initialized successfully.");
            #else
                Debug.Log("Creating WebView for Editor or macOS...");
                webView = await _createWebViewForEditor();
                Debug.Log("WebView for Editor or macOS initialized successfully.");
            #endif

            if (webView == null) {
                Debug.LogError("WebView initialization returned null.");
                return;
            }

            Debug.Log("Loading URL: https://www.google.com...");
            webView.LoadUrl("https://www.google.com");
            Debug.Log("URL loaded successfully.");
        } catch (System.Exception ex) {
            Debug.LogError($"An error occurred during WebView initialization: {ex.Message}\n{ex.StackTrace}");
        }

        // Check runtime assets to catch potential issues
        Debug.Log("Checking runtime assets...");
        CheckForMissingAssets();
    }

    private void CheckForMissingAssets() {
        try {
            // Check for missing or invalid materials
            var materials = Resources.FindObjectsOfTypeAll<Material>();
            foreach (var material in materials) {
                if (material.HasProperty("_MainTex") && material.mainTexture == null) {
                    Debug.LogWarning($"Material '{material.name}' has no main texture assigned.");
                }
            }

            // Log details about Texture2D assets
            var textures = Resources.FindObjectsOfTypeAll<Texture2D>();
            if (textures.Length == 0) {
                Debug.LogError("No Texture2D assets found in the project.");
            } else {
                Debug.Log($"Found {textures.Length} Texture2D assets.");
            }
        } catch (System.Exception ex) {
            Debug.LogError($"An error occurred while checking for missing assets: {ex.Message}\n{ex.StackTrace}");
        }
    }

    private async Task<IWebView> _createWebViewForEditor() {
        Debug.Log("Initializing WebViewPrefab for Editor or macOS...");
        try {
            var webViewPrefab = WebViewPrefab.Instantiate(0.6f, 0.4f);
            webViewPrefab.transform.parent = transform;
            webViewPrefab.transform.localPosition = new Vector3(0, 0.2f, 0.4f);
            webViewPrefab.transform.localEulerAngles = new Vector3(0, 180, 0);

            await webViewPrefab.WaitUntilInitialized();
            Debug.Log("WebViewPrefab initialized successfully.");
            return webViewPrefab.WebView;
        } catch (System.Exception ex) {
            Debug.LogError($"Failed to initialize WebViewPrefab: {ex.Message}\n{ex.StackTrace}");
            return null;
        }
    }
}