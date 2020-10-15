﻿using UnityEngine;

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

		void Awake()
		{
			currentNode = controller.GetStartNode();
		}

        void OnEnable()
        {
			SaveManager.OnGameDataLoaded += SetLoadedData;
		}

        void OnDisable()
        {
			SaveManager.OnGameDataLoaded -= SetLoadedData;
        }

        void SetLoadedData(SaveManager.SaveData loadedData)
        {
			currentNode = null;
			currentNode = controller.GetNode(loadedData.currentNodeGUID);

			if (currentNode == null)
				Debug.LogError("Requested node was not found in current controller");
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