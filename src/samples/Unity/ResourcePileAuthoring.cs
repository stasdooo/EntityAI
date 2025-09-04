using GoalAI.Core.Resources;
using UnityEngine;


public class ResourcePileAuthoring:MonoBehaviour
{
    public int amount = 5;

    private ResourceNode node;

    private void Start()
    {
        var w = AiWorldBootstrap.Instance.World;
        node = new ResourceNode(ResourceType.FoodPile, transform.position.x,transform.position.y,amount);
        w.Resources.Add(node);

        GetComponent<Renderer>().material.color = Color.green;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, 0.2f);

#if UNITY_EDITOR
        if(node != null )
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1f+ Vector3.right*0.5f, "Food: "+ node.Amount);
#endif
    }

}
