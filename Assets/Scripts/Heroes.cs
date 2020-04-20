using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;


public class Heroes : MonoBehaviour
{
    /* UNITY GLUE */
    [Tooltip("Prefab of a UI element that will be drawn near the unit on the battlefield and show \"quantity\" in a unit.")]
    public Text unitQuantityElement;
    [Tooltip("Parent UI GameObject for quantity UI elements.")]
    public GameObject battlefieldUIParent;

    /* END UNITY GLUE */

    
    /* CONFIG */
    [HideInInspector]
    [Tooltip("Time the screeen fades in from black to the first UI screen.")]
    public float startingFadeInTime = 0.0f;     // disabled for now

    public UnitType[] unitTypes;

    //public int[] levelExperience;

    public List<Skill> skills;


    /* HACK */
    [Tooltip("Prefab for small obsctale object")]
    public GameObject smallObstacle;
    [Tooltip("Prefab for large obsctale object")]
    public GameObject largeObstacle;
    /* END HACK */

    
    /* END CONFIG */

    
    /* RUNTIME DATA */
    GameSession session;

    /* RUNTIME DATA */

    
    /* VIEWS */
    [Header("Views")]
    [Tooltip("Logo view for introduction.")]
    public LogoView logoView;

    [Tooltip("Choose faction view.")]
    public LogoView chooseFactionView;

    [Tooltip("UI view for unit \"shop\" phase of the game.")]
    public ShopView shopView;
    
    [Tooltip("Parent GameObject for battle phase of the game.")]
    public Battlefield battlefield;

    /* END VIEWS */

    
    private float startTime;
    private bool started = false;
    
    void Awake()
    {
        logoView.Activate(false);
        chooseFactionView.Activate(false);
        shopView.Activate(false);
        battlefield.Activate(false);
    }
    

    void Start()
    {
        G.game = this;

        startTime = Time.time;

        InitUnitTypes();

        InitSkills();
    }


    void Update()
    {
        if (Time.time - startTime > startingFadeInTime && !started)
        {
            ActivateLogoView();
            started = true;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ApplicationUtil.Quit();
        }
    }


    public void StartNewSession()
    {
        G.session = new DungeonHeroesGameSession();
    }


    public void ActivateLogoView()
    {
        logoView.Activate(true);
        chooseFactionView.Activate(false);
        shopView.Activate(false);
        battlefield.Activate(false);
    }


    public void ActivateChooseFactionView()
    {
        logoView.Activate(false);
        chooseFactionView.Activate(true);
        shopView.Activate(false);
        battlefield.Activate(false);
    }


    public void ActivateShopView()
    {
        logoView.Activate(false);
        chooseFactionView.Activate(false);
        shopView.Activate(true);
        battlefield.Activate(false);
    }


    public void ActivateBattlefieldView()
    {
        logoView.Activate(false);
        chooseFactionView.Activate(false);
        shopView.Activate(false);
        battlefield.Activate(true);
    }


    public void LogoAnimationFinished()
    {
        ActivateChooseFactionView();
    }


    void InitUnitTypes()
    {
        unitTypes = new UnitType[2];

        unitTypes[0] = new UnitType
        {
            name = "Skeleton",
            unitCardPrefab = Resources.Load<GameObject>("Prefabs/UI/UnitCards/Skeleton"),
            gamePrefab = Resources.Load<UnitFE>("Prefabs/Battlefield/Units/Skeleton"),
            data = new UnitData
            {
                cost = 1,
                speed = 3,
                attack = 3,
                defence = 3,
                minDamage = 1,
                maxDamage = 2,
                health = 6
            }
        };

        unitTypes[1] = new UnitType
        {
            name = "Ghost",
            unitCardPrefab = Resources.Load<GameObject>("Prefabs/UI/UnitCards/Ghost"),
            gamePrefab = Resources.Load<UnitFE>("Prefabs/Battlefield/Units/Ghost"),
            data = new UnitData
            {
                cost = 3,
                speed = 6,
                attack = 10,
                defence = 0,
                minDamage = 3,
                maxDamage = 5,
                health = 6
            }
        };

        unitTypes[2] = new UnitType
        {
            name = "Viking",
            unitCardPrefab = Resources.Load<GameObject>("Prefabs/UI/UnitCards/Viking"),
            gamePrefab = Resources.Load<UnitFE>("Prefabs/Battlefield/Units/Viking"),
            data = new UnitData
            {
                cost = 8,
                speed = 3,
                attack = 10,
                defence = 10,
                minDamage = 4,
                maxDamage = 6,
                health = 20
            }
        };
    }


    void InitSkills()
    {
        //skills = new List<Skill>();

        //Skill s = new Skill();
        //s.levels.Add(new SkillLevel {
        //    name = "Basic Logistics",
        //    description = "Increases movement range by 10%",
        //});
        //s.levels.Add(new SkillLevel {
        //    name = "Advanced Logistics",
        //    description = "Increases movement range by 20%",
        //});
        //s.levels.Add(new SkillLevel {
        //    name = "Expert Logistics",
        //    description = "Increases movement range by 30%",
        //});

        //skills.Add(s);
    }


    public void StartBattle()
    {
        battlefield.InitBattle();
    }


    public delegate void SimpleFunc();

    public IEnumerator ExecuteNextFrame(SimpleFunc actualFunc)
    {
        yield return new WaitForSeconds(0);
        actualFunc();
    }
}
