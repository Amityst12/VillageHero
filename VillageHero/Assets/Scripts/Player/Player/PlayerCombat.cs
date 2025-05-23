using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    private PlayerReferences refs;
    private bool isAttacking = false;

    void Start()
    {
        refs = GetComponent<PlayerReferences>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        Quaternion originalRotation = refs.axePivot.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, -120f);

        float elapsed = 0f;
        while (elapsed < refs.attackDuration)
        {
            float t = elapsed / refs.attackDuration;
            refs.axePivot.localRotation = Quaternion.Lerp(originalRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        refs.axePivot.localRotation = originalRotation;

        GameObject attackArea = Instantiate(refs.attackAreaPrefab, refs.attackPoint.position, Quaternion.identity);
        attackArea.transform.parent = transform;

        yield return new WaitForSeconds(refs.attackDuration);
        Destroy(attackArea);
        isAttacking = false;
    }
}
