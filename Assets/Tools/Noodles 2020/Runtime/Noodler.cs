using UnityEngine;

namespace nullbloq.Noodles
{
	/// <summary>
	/// Works like an animator component, contains the Noodle that it controls.
	/// TODO Should have methods like, SetTrigger, SetFloat, etc, or something like that.
	/// </summary>
	public class Noodler : MonoBehaviour
	{
		public Noodle controller;
		public bool startOnAwake;
		public bool playing;
		NoodlesNode currentNode;

		public NoodlesNode CurrentNode
		{
			get
			{
				if (currentNode == null)
					currentNode = controller.GetStartNode();
				return currentNode;
			}
		}

        void OnEnable()
        {
			StoryManager.OnNoodlerControllerSet += SetLoadedNode;
		}

        void OnDisable()
        {
			StoryManager.OnNoodlerControllerSet -= SetLoadedNode;
        }

        void SetLoadedNode(string nodeGUID)
        {
			currentNode = null;
			currentNode = controller.GetNode(nodeGUID);

			if (currentNode == null)
				currentNode = controller.GetStartNode();
        }

        public bool HasNextNode()
		{
			return CurrentNode.HasAnyOutput();
		}

		public NoodlesNode Next(int index)
		{
			currentNode = controller.GetNode(currentNode.GetNextNodeID(index));
			return currentNode;
		}

		public void ResetNoodle()
        {
			currentNode = controller.GetStartNode();
		}
	}
}