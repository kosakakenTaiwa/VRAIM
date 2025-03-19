using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace KosakaKen.DictationAI.Scripts.Client
{
    public class ClientFace : MonoBehaviour
    {
        [Serializable]
        public class Part
        {
            public FacePart FacePart;
            public Transform Transform;
        }
        
        /// <summary>
        /// ClientFaceのシングルトン
        /// </summary>
        public static ClientFace Instance;
        
        [SerializeField] private List<Part> _faceParts = new();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// 指定した部分のTransformを取得する。
        /// </summary>
        /// <param name="targetParts"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Transform GetFacePartTransform(FacePart targetParts)
        {
            foreach (var facePart in _faceParts)
            {
                if (facePart.FacePart == targetParts)
                    return facePart.Transform;
            }

            throw new ArgumentException($"{targetParts} is not set. Please check if {targetParts} is set from the inspector.");
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}