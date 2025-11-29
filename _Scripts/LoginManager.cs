using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace KutumbAssignment
{
    public class LoginManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button appleLoginButton;
        [SerializeField] private TextMeshProUGUI errorMessageText;
        [SerializeField] private GameObject errorMessagePanel;

        [Header("Scene Settings")]
        [SerializeField] private string humanoidSceneName = "HumanoidScene";

        [Header("Debug Settings")]
        [SerializeField] private bool simulateFailure = false;

        private void Start()
        {
            if (appleLoginButton != null)
            {
                appleLoginButton.onClick.AddListener(OnAppleLoginPressed);
            }

            if (errorMessagePanel != null)
            {
                errorMessagePanel.SetActive(false);
            }
        }

        private void OnAppleLoginPressed()
        {
            bool loginSuccessful = ValidateLogin();

            if (loginSuccessful)
            {
                LoadHumanoidScene();
            }
            else
            {
                ShowErrorMessage("Invalid login");
            }
        }

        private bool ValidateLogin()
        {
            return !simulateFailure;
        }

        private void ShowErrorMessage(string message)
        {
            if (errorMessageText != null)
            {
                errorMessageText.text = message;
            }

            if (errorMessagePanel != null)
            {
                errorMessagePanel.SetActive(true);
                Invoke(nameof(HideErrorMessage), 3f);
            }
        }

        private void HideErrorMessage()
        {
            if (errorMessagePanel != null)
            {
                errorMessagePanel.SetActive(false);
            }
        }

        private void LoadHumanoidScene()
        {
            try
            {
                SceneManager.LoadScene(humanoidSceneName);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load scene {humanoidSceneName}: {e.Message}");
                ShowErrorMessage($"Failed to load scene: {e.Message}");
            }
        }

        private void OnDestroy()
        {
            if (appleLoginButton != null)
            {
                appleLoginButton.onClick.RemoveListener(OnAppleLoginPressed);
            }
        }
    }
}