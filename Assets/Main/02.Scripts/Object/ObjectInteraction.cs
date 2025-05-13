using System.Collections;
using UnityEngine;

public enum ObjectType
{
    Boom,
    Ice,
    Breakable,
}

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] GameObject EffectFrefab;
    [SerializeField] ObjectType _objectType;

    public ObjectType ObjectType => _objectType;

    public void ActiveObject()
    {
        switch (_objectType)
        {
            case ObjectType.Boom:
                ActiveBoom();
                break;
            case ObjectType.Ice:
                ActiveIce();
                break;
            case ObjectType.Breakable:
                ActiveBreakable();
                break;
        }
    }
    public void ActiveBoom()
    { StartCoroutine(C_Boom()); }
    public void ActiveIce()
    { StartCoroutine(C_Ice()); }
    IEnumerator C_Boom()
    {
        GameObject Effect = Instantiate(EffectFrefab);
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        Effect.transform.position = transform.position;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }
    IEnumerator C_Ice()
    {
        GameObject Effect = Instantiate(EffectFrefab);
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        Effect.transform.position = transform.position;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }
    public void ActiveBreakable() 
    {
         GameObject Effect = Instantiate(EffectFrefab);
         Effect.transform.position = transform.position;
         Destroy(gameObject);
    }

}
