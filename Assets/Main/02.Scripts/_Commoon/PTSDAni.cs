using UnityEngine;

public class PTSDAni
{
    public static void UpdateAny(Animator ani, Vector2 dir)
    {
        ani.SetFloat("X", dir.x);

        if (dir == Vector2.zero)
        {
            ani.SetBool("Is MoveX", false);
            ani.SetBool("Is MoveY", false);
        }
        else if(dir != Vector2.zero)
        {
            if (dir.x == 0)
            { ani.SetBool("Is MoveY", true); }
            else if(dir.x != 0)
            { 
                ani.SetBool("Is MoveX", true);
                ani.SetFloat("Idle X", dir.x);
            }
        }
    }
    public static void UpdateTroller(Animator ani, Vector2 dir, float mag)
    {
        if(ani.GetBool("Is Active"))
        {
            ani.SetFloat("X", dir.x);

            if (mag <= 0f)
            {
                ani.SetBool("Is MoveX", false);
                ani.SetBool("Is MoveY", false);
            }
            else if (mag > 0f)
            {
                if (dir.x == 0)
                { ani.SetBool("Is MoveY", true); }
                else if (dir.x != 0)
                {
                    ani.SetBool("Is MoveX", true);
                    ani.SetFloat("Idle X", dir.x);
                }
            }
        }
    }
}
