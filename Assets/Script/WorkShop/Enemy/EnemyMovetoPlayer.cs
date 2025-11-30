using UnityEngine;

public class EnemyMovetoPlayer : Enemy
{
    protected override void Update()
    {
        base.Update();

        if (player == null)
        {
            if (animator != null)
                animator.SetBool("Attack", false);
            return;
        }

        Turn(player.transform.position - transform.position);
        timer -= Time.deltaTime;

        if (GetDistanPlayer() < 1.5f)
        {
            Attack(player);
        }
        else
        {
            if (animator != null)
                animator.SetBool("Attack", false);

            Vector3 direction = (player.transform.position - transform.position).normalized;
            Move(direction);
        }
    }
}

