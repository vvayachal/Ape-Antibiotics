using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using System.Linq.Expressions;
using UnityEditor.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dash : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    public bool dashRequest;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    //this is for the custom part of the inspector
    [HideInInspector]
    public bool canBash;
    [HideInInspector]
    public LayerMask enemyLayers = 7;
    [HideInInspector]
    public Transform bashHitbox;
    [HideInInspector]
    public float bashDamage;
    [HideInInspector]
    public float bashRange;

    private Knockback knockb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        knockb = GetComponent<Knockback>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            dashRequest = true;
        }

        if (dashCdTimer >0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (dashRequest)
        {
            Dashing();

            dashRequest = false;
        }
    }

    private void Dashing()
    {
        if (dashCdTimer > 0)
        {
            return;
        }
        else
        {
            dashCdTimer = dashCd;
        }

        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        rb.AddForce(forceToApply, ForceMode.Impulse);
        
        if(canBash)
        {
            bash();
        }

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void bash()
    {
        Debug.Log("BASHING -" + " Dmg: " + bashDamage + " Range: " + bashRange);

        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(bashHitbox.position, bashRange, enemyLayers);

        Debug.Log(hitEnemies.Length);

        foreach (Collider enemy in hitEnemies)
        {
            // pretty inefficient because we're using
            // getcomponent twice but idk how to really optimize this
            Debug.Log($"{this.name} has bashed {enemy.name} away!");
            if (enemy.gameObject.GetComponent<EnemyManager>() != null)
            {
                enemy.gameObject.GetComponent<EnemyManager>().TakeDamage(bashDamage);
            }

            // Knock them back
            StartCoroutine(knockb.ApplyKnockBack(enemy, bashHitbox.position, bashDamage));
        }
    }

    private void ResetDash()
    {
        Debug.Log("Dash Reset");
    }

    private void OnDrawGizmosSelected()
    {
        if (bashHitbox == null) return;

        if (bashHitbox.parent.gameObject.activeSelf)
        {
            Gizmos.DrawWireSphere(bashHitbox.position, bashRange);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Dash))]
public class Dash_Editor : Editor
{ 
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Dash dashScript = (Dash)target;

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Bash", EditorStyles.boldLabel);
        serializedObject.FindProperty("canBash").boolValue = EditorGUILayout.Toggle("Can Bash", dashScript.canBash);

        if (dashScript.canBash)
        {
            serializedObject.FindProperty("bashHitbox").objectReferenceValue = EditorGUILayout.ObjectField("Bash Hitbox", dashScript.bashHitbox, typeof(Transform), true) as Transform;
            serializedObject.FindProperty("bashDamage").floatValue = EditorGUILayout.FloatField("Bash Damage", dashScript.bashDamage);
            serializedObject.FindProperty("bashRange").floatValue = EditorGUILayout.FloatField("Bash Range", dashScript.bashRange);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif