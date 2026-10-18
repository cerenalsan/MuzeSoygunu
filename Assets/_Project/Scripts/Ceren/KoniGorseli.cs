using UnityEngine;

namespace MuzeSoygunu.Ceren
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class KoniGorseli : MonoBehaviour
    {
        [SerializeField] private GorusKonisi konisi;
        [SerializeField] private int dilimSayisi = 10;
        [SerializeField] private Color renk = new Color(1f, 0.15f, 0.15f, 0.35f);

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = KoniOlustur();

            MeshRenderer cizici = GetComponent<MeshRenderer>();
            cizici.material = new Material(Shader.Find("Sprites/Default")) { color = renk };
            cizici.sortingOrder = -5;
        }

        private Mesh KoniOlustur()
        {
            Vector3[] noktalar = new Vector3[dilimSayisi + 2];
            int[] ucgenler = new int[dilimSayisi * 3];

            noktalar[0] = Vector3.zero;
            for (int i = 0; i <= dilimSayisi; i++)
            {
                float oran = (float)i / dilimSayisi;
                float aci = Mathf.Lerp(-konisi.YariAci, konisi.YariAci, oran) * Mathf.Deg2Rad;
                noktalar[i + 1] = new Vector3(Mathf.Cos(aci), Mathf.Sin(aci), 0f) * konisi.Menzil;
            }

            for (int i = 0; i < dilimSayisi; i++)
            {
                ucgenler[i * 3] = 0;
                ucgenler[i * 3 + 1] = i + 2;
                ucgenler[i * 3 + 2] = i + 1;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = noktalar;
            mesh.triangles = ucgenler;
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}