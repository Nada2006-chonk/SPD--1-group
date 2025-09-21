using UnityEngine;

public class LightConeDetector : MonoBehaviour
{

    [SerializeField] private float lightRange = 5f; //samma value som light range i light sourcen
    [SerializeField] private float lightAngle = 45f; //sätt inner angle till samma och sen outer lite mer
    [SerializeField] private int rayCount = 15;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask targetMask;


    [HideInInspector] public bool playerInLight;

    private void Update()
    {
        playerInLight = false;

        
        Vector2 origin = transform.position;
        Vector2 forward = -transform.up; //default vectorn pekar rakt ned

      
        float halfAngle = lightAngle / 2f; 
        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1); // sprid raycounts jämt mellan 0-1
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);
            //-halfangle = -22.5 halfangle = 22.5.
            //Sprid vårat t värde som är 0-1 till ett som är -22.5 och 22.5 för att distribuera raysen korrekt
            Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;
            //rotera runt Z axeln i fraktioner på angle värdet.
            //multiplicera med forward som är en vektor som pekar rakt ned
            //betyder typ att man roterar forward vektorn runt Z axeln angle grader
            //första iterationen vrids forward -22.5 grader och sista +22.5 grader.
            


            RaycastHit2D hit = Physics2D.Raycast(origin, dir, lightRange, obstructionMask | targetMask);

            if (hit.collider != null)
            {
                
                if (((1 << hit.collider.gameObject.layer) & obstructionMask) != 0)
                {
                    continue;
                }

                
                if (((1 << hit.collider.gameObject.layer) & targetMask) != 0)
                {
                    playerInLight = true;
                    break;
                }
            }

            
            Debug.DrawRay(origin, dir * lightRange, playerInLight ? Color.green : Color.yellow);
        }


    }

    

}
