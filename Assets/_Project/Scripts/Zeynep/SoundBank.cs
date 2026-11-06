using System;
using UnityEngine;

namespace MuzeSoygunu.Llm
{
    // SoundId'leri ses dosyalarına bağlayan ayar dosyası. Eksik bir ses hata değil, sessizlik demektir.
    [CreateAssetMenu(fileName = "SoundBank", menuName = "MuzeSoygunu/Sound Bank")]
    public class SoundBank : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public SoundId id;
            public AudioClip clip;
        }

        [SerializeField] private Entry[] entries = new Entry[0];

        public AudioClip Get(SoundId id)
        {
            if (entries == null)
            {
                return null;
            }

            foreach (Entry entry in entries)
            {
                if (entry.id == id)
                {
                    return entry.clip;
                }
            }

            return null;
        }
    }
}
