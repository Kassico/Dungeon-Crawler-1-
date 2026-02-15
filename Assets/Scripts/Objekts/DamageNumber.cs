using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{

    private Rigidbody2D rb;
    private TMP_Text dmgValue;


    

    public float initialYVelocity = 3.5f;
    public float initialXVelocityRange = 1.5f;
    public float lifeTime = 0.8f;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dmgValue = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {   
        rb.linearVelocity = new Vector2(Random.Range(-initialXVelocityRange, initialXVelocityRange), initialYVelocity);
        Destroy(gameObject, lifeTime);
        PlayerAttacks playerAttacks = FindObjectOfType<PlayerAttacks>();
        if (playerAttacks != null)
        {
            DmgText(playerAttacks.playerDmg);
        }
    }
    public void DmgText(float damageAmount)
    {
        dmgValue.text = damageAmount.ToString();
    }

    //public void Setup(float damageAmount)
    //{
    //    dmgValue.text = damageAmount.ToString();
    //    rb.linearVelocity = new Vector2(Random.Range(-initialXVelocityRange, initialXVelocityRange), initialYVelocity);
    //    Destroy(gameObject, lifeTime);
    //}
    //public void Setup(float damageAmount)
    //{
    //    text.text = damageAmount.ToString();
    //}

    //private void Update()
    //{
    //    transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    //    timer += Time.deltaTime;
    //    float t = timer / lifeTime;

    //    if (timer >= lifeTime)
    //    {
    //        Destroy(gameObject);
    //    }

    //}

    //public IEnumerator DmgText(GameObject DmgObj)
    //{ Vector3 startPos = DmgObj.transform.position;
    //    Vector3 endPos = startPos + Vector3.up * 1.5f;
    //    float elapsedTime = 0f;
    //    float duration = 0.7f;
    //    while (elapsedTime < duration)
    //    {
    //        DmgObj.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
    //        DmgObj.GetComponent<TextMeshPro>().color = new Color(1f, 0f, 0f, 1f - (elapsedTime / duration));
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    Destroy(DmgObj);
    //}

}
