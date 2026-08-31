using UnityEngine;
using UnityEngine.Playables;

public class LevelIntroController : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector timeline;
    public ThirdPersonCamera playerCamera;
    public Camera mainCamera;

    private Behaviour cinemachineBrain;
    private bool cutsceneFinished = false;

    private void Start()
    {
        // Find the Cinemachine Brain automatically.
        if (mainCamera != null)
        {
            cinemachineBrain =
                mainCamera.GetComponent("CinemachineBrain") as Behaviour;
        }

        if (cinemachineBrain == null)
        {
            Debug.LogError(
                "LevelIntroController: Could not find Cinemachine Brain on Main Camera!"
            );
        }

        // Disable normal third-person camera control
        // while the cinematic is playing.
        if (playerCamera != null)
        {
            playerCamera.StartCinematic();
        }
    }

    private void Update()
    {
        if (cutsceneFinished)
            return;

        if (timeline == null)
            return;

        // Check whether the Timeline has reached its end.
        if (timeline.time >= timeline.duration - 0.05f)
        {
            EndCutscene();
        }
    }

    private void EndCutscene()
    {
        cutsceneFinished = true;

        Debug.Log("=================================");
        Debug.Log("LEVEL INTRO FINISHED");
        Debug.Log("Disabling Cinemachine Brain...");
        Debug.Log("=================================");

        // Disable Cinemachine Brain.
        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = false;

            Debug.Log(
                "Cinemachine Brain enabled: " +
                cinemachineBrain.enabled
            );
        }

        // Make absolutely sure the Main Camera is enabled.
        if (mainCamera != null)
        {
            mainCamera.enabled = true;
        }

        // Return control to the normal third-person camera.
        if (playerCamera != null)
        {
            playerCamera.EndCinematic();
        }
    }

    private void OnDestroy()
    {
        // Nothing needed here because we're no longer
        // relying on the Timeline stopped event.
    }
}