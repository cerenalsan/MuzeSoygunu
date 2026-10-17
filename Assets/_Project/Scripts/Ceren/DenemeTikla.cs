using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class DenemeTikla : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 ekran = Mouse.current.position.ReadValue();
            Vector3 dunya = Camera.main.ScreenToWorldPoint(ekran);
            dunya.z = 0f;
            agent.SetDestination(dunya);
        }
    }
}