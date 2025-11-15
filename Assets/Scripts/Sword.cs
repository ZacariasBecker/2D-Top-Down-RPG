using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");

        // CORREÇÃO: Verifica se as referências cruciais estão atribuídas.
        // Isso previne a UnassignedReferenceException.
        if (slashAnimSpawnPoint != null && slashAnimPrefab != null)
        {
            slashAnim = Instantiate(slashAnimPrefab,
                                    slashAnimSpawnPoint.position,
                                    Quaternion.identity);

            slashAnim.transform.parent = this.transform.parent;
        }

    }


    public void SwingUpFlipAnim()
    {
        // CORREÇÃO: Adiciona verificação para 'slashAnim', caso Attack() não tenha sido chamado
        // ou a instanciação falhou devido a uma referência 'null'.
        if (slashAnim != null)
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

            if (playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    public void SwingDownFlipAnim()
    {
        // CORREÇÃO: Adiciona verificação para 'slashAnim'.
        if (slashAnim != null)
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void MouseFollowWithOffset()
    {
        // Código comentado. Não há erros de sintaxe aqui.
    }
}