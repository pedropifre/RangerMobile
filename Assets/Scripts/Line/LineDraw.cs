using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ebac.Core.Singleton;
using System.Linq;
using SystemSFX;
using TMPro;


public class LineDraw : Singleton<LineDraw>
{


    public List<Vector2> pointsList;


    public GameObject GuardPrefab;
    public GameObject curentGuard;
    public int lineLenghtMax = 70;

    [Header("Enemys in Screen")]
    public List<GameObject> EnemyOBJ;

    [SerializeField] private LineRenderer line;
    private EdgeCollider2D edgecollider;

    private Material line_material;
    private AudioSource audioSource;

    private Vector3 mousePos;
    private Vector3 fingerpos_halfpoint, fingerpos_startpoint;

    private bool isMousePressed;
    private int index;
    private string styler_color;

    [Header("Audio Play")]
    public SFXManager sFXManager;
    public AudioSource sFXPlaceStyler;
    public AudioSource sFXBreakStyler;

    [Header("Audio Play")]
    public ParticleSystem particleBreak;

    [Header("Damage text")]
    public float damageTotal;
    public GameObject textDamage;

    [Header("Manager")]
    public _SOPlayer SOPlayer;
    private CalculateDamage calculateDamage;
    // Structure for line points
    struct myLine { public Vector3 StartPoint; public Vector3 EndPoint; public Vector3 HalfPoint; };

    void Awake()
    {

        line = gameObject.GetComponent<LineRenderer>();
        edgecollider = this.GetComponent<EdgeCollider2D>();
        line.useWorldSpace = true;
        isMousePressed = false;
        pointsList = new List<Vector2>();

        audioSource = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        mousePos.z = 0;
        
        if (Input.GetMouseButtonDown(0) && Input.touchCount < 2)
        {
            sFXPlaceStyler.clip = sFXManager.PlaySFX("MetalStyler");
            sFXPlaceStyler.Play();
            isMousePressed = true;
            audioSource.pitch = 1;
            audioSource.Play();
            curentGuard = Instantiate(GuardPrefab, Vector3.zero, Quaternion.identity);
        }

        if (Input.GetMouseButtonUp(0) || Input.touchCount >= 2)
        {
            DestroyElement();
            
        }

        if (isMousePressed && StylerCheck())
        {
            edgecollider.enabled = true;
            SetLine();
            LengthLineCheck();
            if (isLineCollide()) PlacementCenter();
        }
    }

    bool StylerCheck()
    {
        if (curentGuard) return true;
        else return false;
    }

    private void SetLine()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (!pointsList.Contains(mousePos))
        {
            pointsList.Add(mousePos);
            line.positionCount = pointsList.Count;
            line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
            edgecollider.points = pointsList.ToArray();
            edgecollider.offset = new Vector2(this.transform.position.x * -1, this.transform.position.y * -1);

            curentGuard.transform.position = new Vector2(line.GetPosition(pointsList.Count - 1).x, line.GetPosition(pointsList.Count - 1).y);
            this.transform.position = curentGuard.transform.position;
            if (audioSource.pitch < 10) audioSource.pitch += 0.002f;
        }
        else
        {
            if (audioSource.pitch > 1) audioSource.pitch -= 0.002f;
        }
    }

    private void LengthLineCheck()
    {
        
        if (pointsList.Count > lineLenghtMax)
        {
            DestroyElement();
        }
        else return;
    }
    private bool isLineCollide()
    {
        if (pointsList.Count < 2)
            return false;
        int TotalLines = pointsList.Count - 1; myLine[] lines = new myLine[TotalLines];

        if (TotalLines > 1)
        {
            for (int i = 0; i < TotalLines; i++)
            {
                lines[i].StartPoint = (Vector3)pointsList[i];
                lines[i].EndPoint = (Vector3)pointsList[i + 1];
            }
        }

        for (int i = 0; i < TotalLines - 1; i++)
        {
            myLine currentLine;
            currentLine.StartPoint = (Vector3)pointsList[pointsList.Count - 2];
            currentLine.EndPoint = (Vector3)pointsList[pointsList.Count - 1];
            currentLine.HalfPoint = (Vector3)pointsList[pointsList.Count / 2];
            if (isLinesIntersect(lines[i], currentLine))
            {
                pointsList.RemoveRange(0, pointsList.Count);
                index = i; if (index <= 0) index = 1; line.positionCount = index;
                fingerpos_startpoint = currentLine.StartPoint;
                fingerpos_halfpoint = currentLine.HalfPoint;
                SetLine();
                return true;
            }
        }
        return false;
    }

    //    Following method checks whether given two points are same or not  
    private bool checkPoints(Vector3 pointA, Vector3 pointB) { return (pointA.x == pointB.x && pointA.y == pointB.y); }

    //    Following method checks whether given two line intersect or notW
    private bool isLinesIntersect(myLine L1, myLine L2)
    {
        if (checkPoints(L1.StartPoint, L2.StartPoint) || checkPoints(L1.StartPoint, L2.EndPoint) ||
            checkPoints(L1.EndPoint, L2.StartPoint) || checkPoints(L1.EndPoint, L2.EndPoint))
            return false;

        return ((Mathf.Max(L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min(L2.StartPoint.x, L2.EndPoint.x)) &&
               (Mathf.Max(L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min(L1.StartPoint.x, L1.EndPoint.x)) &&
               (Mathf.Max(L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min(L2.StartPoint.y, L2.EndPoint.y)) &&
               (Mathf.Max(L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min(L1.StartPoint.y, L1.EndPoint.y)));
    }

    //THE METHOD SOTTO ARE FOR THE CATCH POKEMON SYSTEM

    //    Following method Detect if a pokemon is inside of a circle
    void DetectedPokemon(Vector3 circle_position, float raggio)
    {
        //battleManager.InsideCircle();
        foreach(var EnemyTime in EnemyOBJ.ToList())
        {

            float dist = Mathf.Sqrt(((circle_position.x - EnemyTime.transform.position.x) * (circle_position.x - EnemyTime.transform.position.x)) +
                                    ((circle_position.y - EnemyTime.transform.position.y) * (circle_position.y - EnemyTime.transform.position.y)));

            if (dist + EnemyTime.GetComponent<CircleCollider2D>().radius <= raggio)
            {
                //Enemy detected
                
                //check if the enemy is underground
                if (EnemyTime.GetComponent<EnemyMovement>().undergroundState == false)
                {
                    //calcular damage
                    GetDamage();
                    EnemyTime.GetComponent<EnemyBase>().DamageEnemy((int)damageTotal);
                    GameObject txtD = Instantiate(textDamage, EnemyTime.transform);
                    //mostrar vida restante
                    //txtD.GetComponent<TextDamageBase>().health = (int)EnemyTime.GetComponent<HealthBase>()._currentLife;
                    //mostar dano sofrido -------- trocar dano pra personalizar com multiplicador
                    txtD.GetComponent<TextDamageBase>().health = (int)damageTotal;
                    txtD.GetComponent<TextDamageBase>().enemy = EnemyTime;
                    txtD.GetComponent<TextDamageBase>().spawn();
                
                }
                
                //break;
            }
        }


    }

    public void GetDamage()
    {
        //base
        damageTotal = 1;
        //Equipament
        damageTotal += (int)CalculateDamage.Instance.damage();
        //PowerUp
    }
 
    //    Following method Calculate the Close Circle When the pokemon was detected
    void PlacementCenter()
    {
        //placement_vector = central position
        Vector3 placement_vector = new Vector3((fingerpos_startpoint.x + fingerpos_halfpoint.x) / 2, (fingerpos_startpoint.y + fingerpos_halfpoint.y) / 2, 1);
        //calculate the diametro
        float diametro = Vector3.Distance(placement_vector, fingerpos_startpoint) * 2;
        DetectedPokemon(placement_vector, diametro / 2);
    }

    //    Following method Detect if the line collide with pokemon or wall


    //    Following method Detect if the line enter in trigger with wall
    private void OnTriggerEnter2D(Collider2D otherCollider)
    { 
        if (otherCollider.gameObject.tag == "Bullet" || otherCollider.gameObject.tag == "Obstacle")
        {
            //Debug.Log("meu deus");
            DestroyElement();
        }
        if (otherCollider.gameObject.tag == "Pokemon" || otherCollider.gameObject.tag == "wall")
        {
            
            //PAUSED - spawn particle when line destroyed
            for (var t = 0;t<pointsList.Count;t++)
            {
                if (t % 2 == 0)
                {
                    //StartCoroutine(SpawnParticleBreak(t));
                }
            }
            
            PlayBreakSound();
            DestroyElement();
        }
    }
    IEnumerator SpawnParticleBreak(int v)
    {
        ParticleSystem particle = Instantiate(particleBreak);
        particleBreak.transform.position = new Vector3(pointsList[v].x, pointsList[v].y);
        particleBreak.Play();
        yield return new WaitForSeconds(.5f);
        Destroy(particle);
    }

    public void PlayBreakSound()
    {
        sFXBreakStyler.clip = sFXManager.PlaySFX("BreakStyler");
        sFXBreakStyler.Play();

    }
    //    Following method Detect if the line stay trigger with wall
    private void OnTriggerStay2D(Collider2D otherCollider) 
    { 
        if (otherCollider.gameObject.tag == "wall") DestroyElement();
        
    }


    //    Following method destroy all Line's object 
    public void DestroyElement()
    {
  
        
        Destroy(curentGuard); 
        pointsList.Clear();
        isMousePressed = false; 
        line.SetVertexCount(0);
        pointsList.RemoveRange(0, pointsList.Count);
        edgecollider.Reset();
        edgecollider.enabled = false;
        audioSource.pitch = 1;
        audioSource.Pause();
    }

    #region LIST ENEMYS CHANGE
    public void AddEnemy(GameObject enemyObjAdd)
    {
        EnemyOBJ.Add(enemyObjAdd);
    }
    public void RemoveEnemy(GameObject enemyObjRemove)
    {
        EnemyOBJ.Remove(enemyObjRemove);
    }
    #endregion
}
