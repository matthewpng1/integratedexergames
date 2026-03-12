using UnityEngine;

namespace Unity.Tutorials
{
    /// <summary>
    /// Disabled: Tutorial callbacks for Build And Publish tutorial
    /// </summary>
    [CreateAssetMenu(fileName = "PublishCriteria", menuName = "Tutorials/Microgame/PublishCriteria")]
    class PublishCriteria : ScriptableObject
    {
        // All tutorial-related logic has been removed/disabled.

        // Example stub methods (always return false or default)
        public bool IsNotDisplayingFirstTimeInstructions() => false;
        public bool IsUserLoggedIn() => false;
        public bool IsBuildBeingUploaded() => false;
        public bool IsBuildPublished() => false;
        public bool AtLeastOneBuildIsRegistered() => false;
    }
}
