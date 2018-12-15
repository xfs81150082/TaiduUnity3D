using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TumoCommon.Model;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour {

    #region 自定义
    public static StartMenuController _instance;
    public static string username;
    public static string password;
    public static string phonenumber;
    public static ServerProperty sp;
    public static List<Role> rolelist = null;

    public InputField usernameInputRegister;
    public InputField passwordInputRegister;
    public InputField repasswordInputRegister;
    public InputField phoneNumberInputRegister;

    public InputField usernameInputLogin;
    public InputField passwordInputLogin;
    public InputField characterNameInputLogin;

    private DOTweenAnimation startChildDotweenAnimation;
    private DOTweenAnimation serverListChildDotweenAnimation;
    private DOTweenAnimation userRegisterImageDotweenAnimation;
    public DOTweenAnimation roleSelectChildDotweenAnimation;
    private DOTweenAnimation rolebuildChildDotweenAnimation;
    private DOTweenAnimation userNameImageDoTweenAnimation;


    private bool haveInitServerlist = false;
    public GameObject serveritemRed;
    public GameObject serveritemGreen;
    public  GameObject content;
    public GameObject serverSelectedGo;
    private Transform characterSelectGO;
    public Text userNameStartImageText;
    public  Text serverNameText;
    private Text characterNameText;

    public GameObject[] characterArray;
    //public GameObject[] selectcharterArray;

    public GameObject characterSelected; 
    private LoginController loginController;
    private RegisterController registerController;
    private RoleController roleController;

    #endregion



    #region awake
    private void Awake()
    {
        _instance = this;
        startChildDotweenAnimation = transform.Find("StartChild").gameObject.GetComponent<DOTweenAnimation>();
        serverListChildDotweenAnimation = transform.Find("ServerListChild").gameObject.GetComponent<DOTweenAnimation>();
        userRegisterImageDotweenAnimation = transform.Find("UserRegisterImage").gameObject.GetComponent<DOTweenAnimation>();
        roleSelectChildDotweenAnimation = transform.Find("CharacterSelectChild").gameObject.GetComponent<DOTweenAnimation>();
        rolebuildChildDotweenAnimation = transform.Find("CharacterShowSelectChild").gameObject.GetComponent<DOTweenAnimation>();
        content = transform.Find("ServerListChild/ServerListScrollView/Viewport/Content").gameObject;
        serveritemRed = Resources.Load("ServerName1Image", typeof(GameObject)) as GameObject;
        serveritemGreen = Resources.Load("ServerName2Image", typeof(GameObject)) as GameObject;
        serverSelectedGo = transform.Find("ServerListChild/ServerNameImage").gameObject;
        characterSelectGO = transform.Find("CharacterSelectChild/ChangeCharacterChild/CharacterSelectGO").GetComponent<Transform>();
        userNameStartImageText= transform.Find("StartChild/UserNameImage/UserNameText").GetComponent<Text>();
        serverNameText = transform.Find("StartChild/ServerNameImage/ServerNameText").GetComponent<Text>();
        characterNameText = transform.Find("CharacterSelectChild/CharacterNameImage/CharacterNameText").GetComponent<Text>();
        usernameInputLogin = transform.Find("UserNameImage/UserNameImage").GetComponent<InputField>();
        passwordInputLogin = transform.Find("UserNameImage/UserPasswordImage").GetComponent<InputField>();
        usernameInputRegister = transform.Find("UserRegisterImage/UserNImage").GetComponent<InputField>();
        passwordInputRegister = transform.Find("UserRegisterImage/PasswordImage").GetComponent<InputField>();
        repasswordInputRegister = transform.Find("UserRegisterImage/ConfirmPasswordImage").GetComponent<InputField>();
        phoneNumberInputRegister = transform.Find("UserRegisterImage/PhoneNumberImage").GetComponent<InputField>();
        characterNameInputLogin = transform.Find("CharacterShowSelectChild/InputNameImage").GetComponent<InputField>();
        userNameImageDoTweenAnimation = transform.Find("UserNameImage").GetComponent<DOTweenAnimation>();

        loginController = this.GetComponent<LoginController>();
        registerController = this.GetComponent<RegisterController>();
        roleController = this.GetComponent<RoleController>();


        roleController.OnGetRole += OnGetRole;
        roleController.OnAddRole += OnAddRole;
        roleController.OnSelectRole += OnSelectRole;

    }

    // Use this for initialization
    void Start ()
    {
        userNameStartImageText.text = usernameInputLogin.text;  //初始化登录用户名
        username = usernameInputLogin.text;
        password = passwordInputLogin.text;

    }
	
	// Update is called once per frame
	void Update () {  
            
    }

    void OnDestroy()
    {
        if (roleController != null)
        {
            roleController.OnAddRole -= OnAddRole;
            roleController.OnGetRole -= OnGetRole;
            roleController.OnSelectRole -= OnSelectRole;
        }
    }


 #endregion



    #region role事件

    public void OnGetRole(List<Role> roleList)
    {
        StartMenuController.rolelist = roleList;
        //这个是得到角色信息之后的处理
        if (roleList != null && roleList.Count > 0)
        {
            //进入角色显示的界面
            Role role = roleList[0];
            ShowRole(role);
        }
        else
        {
            ShowRoleAddPanel(); //进入角色创建的界面
        }
    }

    public void OnAddRole(Role role)
    {
        if (rolelist == null)
        {
            rolelist = new List<Role>();
        }
        rolelist.Add(role);
        ShowRole(role);
    }

    public void OnSelectRole()
    {
        //导步加载场景1，并将加载变量给operation
        AsyncOperation operation = Application.LoadLevelAsync(1);
        //显示加载场景的进度条
        LoadSceneProgressBar._instance.Show(operation);
    }
    
    public void ShowRole(Role role)
    {
        PhotonEngine.Instance.role = role;
        ShowCharacterselect();
        characterNameText.text = role.Name+" LV."+role.Level;
        int index = -1;
        for (int i = 0; i < characterArray.Length; i++)
        {
            if ((characterArray[i].name.IndexOf("boy") >= 0 && role.IsMan) ||
                (characterArray[i].name.IndexOf("girl") >= 0 && role.IsMan == false))
            {
                index = i;
                break;
            }
        }
        GameObject.Destroy(characterSelectGO.GetComponentInChildren<Animator>().gameObject);
        GameObject.Instantiate(characterArray[index], characterSelectGO.transform);

        //GameObject go = GameObject.Instantiate(characterArray[index]);
        //go.transform.parent = characterSelectGO.transform;
        
        /*GameObject go = null;
        foreach (GameObject character in characterArray)
        {
            if (character == go)
            {
                character.GetComponent<DOTweenAnimation>().DOPlayForward();
            }
            else
            {
                character.GetComponent<DOTweenAnimation>().DOPlayBackwards();
            }
        }
        GameObject.Destroy(characterSelectGO.GetComponentInChildren<Animator>().gameObject);
        GameObject.Instantiate(go, characterSelectGO.transform);
        */

    }

    public void ShowCharacterselect()
    {
        roleSelectChildDotweenAnimation.DOPlayForward();
    }

    public void ShowRoleAddPanel()
    {
        rolebuildChildDotweenAnimation.DOPlayForward();

    }

    #endregion




    #region  ON...Click

    public void OnServerNameImageClick()
    {
        startChildDotweenAnimation.DOPlayBackwards();
        serverListChildDotweenAnimation.DOPlayForward();
    }

    public void OnCancelImageClick()
    {
        serverListChildDotweenAnimation.DOPlayBackwards();
        startChildDotweenAnimation.DOPlayForward();
    }

    public void OnUserRegisterTextClick()
    {
        userNameImageDoTweenAnimation.DOPlayBackwards();
        userRegisterImageDotweenAnimation.DOPlayForward();
    }

    public void OnCancelImage2Click()
    {        
        userRegisterImageDotweenAnimation.DOPlayBackwards();
        userNameImageDoTweenAnimation.DOPlayForward();
    }
    
    public void OnServerselect(GameObject serverGo)
    {       
        sp = serverGo.GetComponent<ServerProperty>();        
        serverSelectedGo.transform.Find("ServerNameText").GetComponent<Text>().text = sp.Name;
        serverSelectedGo.GetComponent<Image>().sprite = serverGo.GetComponent<Image>().sprite;

        if (serverSelectedGo.gameObject.activeSelf == false)
        {
            serverSelectedGo.gameObject.SetActive(true);
        }

    }
    
    public void OnServerpanelClose()
    {
        //隐去服务器列表
        //显示开始界面
        startChildDotweenAnimation.DOPlayForward();
        serverListChildDotweenAnimation.DOPlayBackwards();
        //更新开始画面中的服务名称
        serverNameText.text = sp.Name;
    }

    public void OnLoginClick()
    {
        startChildDotweenAnimation.DOPlayBackwards();
        roleSelectChildDotweenAnimation.DOPlayForward();  //?
        //保存用户名和密码，并登录。
        username = usernameInputLogin.text;
        password = passwordInputLogin.text;
        //验证用户名和密码，验证成功后登录游戏，加载游戏画面。


        //验证用户名和密码，验证失败后无法登录。
        //进入注册页面，注册用户名。
    }

    public void OnRegisterAndLoginClick()
    {
        username = usernameInputRegister.text;
        password = passwordInputRegister.text;
        string rePassword = repasswordInputRegister.text;
        if (username == null || username.Length <= 3)
        {
            MessageManger._instance.ShowMessage("用户名不能为空或少于三个字符",2);
            return;
        }
        if (password == null || password.Length <= 3) 
        {
            MessageManger._instance.ShowMessage("密码不能为空或少于三个字符",2);
            return;
        }
        if (password != rePassword)
        {
            MessageManger._instance.ShowMessage("二次输入密码不一样",2);
            return;
        }
        registerController.Register(username, password, this);


        //1.//toto//2.//toto//3.
        //保存用户名和密码

        //phonenumber = phoneNumberInputRegister.text;
        //验证用户名和密码，验证成功后登录游戏，加载游戏画面。
        //验证用户名和密码，验证失败后无法登录。

    }
    
    

    public void OnCharacterClick(GameObject go)
    {
        if (go == characterSelected)
        {
            return;
        }
        //iTween.ScaleTo(go, new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
        go.GetComponent<DOTweenAnimation>().DOPlayForward();
        if (characterSelected != null)
        {
            //iTween.ScaleTo(characterSelected, new Vector3(1f, 1f, 1f), 0.5f);
            characterSelected.GetComponent<DOTweenAnimation>().DOPlayBackwards();
        }
        characterSelected = go;

        //判断当前选择的角色是否已经创建，通名字判断
        foreach (var role in rolelist)
        {
            if ((role.IsMan && go.name.IndexOf("boy") >= 0) || (role.IsMan == false && go.name.IndexOf("girl") >= 0))
            {
                characterNameInputLogin.text = role.Name;
            }

        }

    }

    public void OnShowRoleBuildPlanelClick()
    {
        roleSelectChildDotweenAnimation.DOPlayBackwards();      //?
        rolebuildChildDotweenAnimation.DOPlayForward();         //?
    }
    
    public void OnCharacterShowSelectCanelClick()
    {
        rolebuildChildDotweenAnimation.DOPlayBackwards();   //
        roleSelectChildDotweenAnimation.DOPlayForward();    //
    }
    

    //角色创建确认按钮
    public void OnRoleBuildPancelConfirmClick()
    {
        rolebuildChildDotweenAnimation.DOPlayBackwards();  //隐藏创建角色的面板
        //roleSelectChildDotweenAnimation.DOPlayForward();   //
        //characterNameText.text = characterNameInputLogin.text;

        //判断角色的名字是否符合要求？
        if (characterNameInputLogin.text.Length < 3)
        {
            MessageManger._instance.ShowMessage("角色的名字不能少于3个字符");
            return;
        }

        //判断当前的角色是否已经创建
        Role role = null;
        foreach (var roleTemp in rolelist)
        {
            if ((roleTemp.IsMan && characterSelected.name.IndexOf("boy") >= 0) || (roleTemp.IsMan == false && characterSelected.name.IndexOf("girl") >= 0))
            {
                characterNameInputLogin.text = roleTemp.Name;
                role = roleTemp;
            }
        }
        if (role == null)
        {
            Role roleAdd = new Role();
            roleAdd.IsMan = characterSelected.name.IndexOf("boy") >= 0 ? true : false;
            roleAdd.Name = characterNameInputLogin.text;
            roleAdd.Level = 1;
            roleAdd.Exp = 0;
            roleAdd.Coin = 20000;
            roleAdd.Diamond = 1000;
            roleAdd.Energy = 100;
            roleAdd.Toughen = 50;
            roleController.AddRole(roleAdd);

        }
        else
        {
            ShowRole(role);
        }
        

    }

    public void OnEnterGameClick()
    {
        //登录，验证用户名和密码
        loginController.Login(username,password);

        //1.加载场景
        //toto
        //2.实体化，选定的角色，带等级，姓名，等属性
        //toto
        
    }

    public void OnUsernamePasswordClick()
    {
        startChildDotweenAnimation.DOPlayBackwards();
        userNameImageDoTweenAnimation.DOPlayForward();

    }

    public void OnTrueClick()
    {
        startChildDotweenAnimation.DOPlayForward();
        userNameImageDoTweenAnimation.DOPlayBackwards();
        username = usernameInputLogin.text;
        password = passwordInputLogin.text;
        userNameStartImageText.text = usernameInputLogin.text;

    }

    public void OnGamePlayClicK()
    {
        roleController.SelectRole(PhotonEngine.Instance.role);
        print(PhotonEngine.Instance.role.Name);
    }

    #endregion




    #region //方法
    /*
    public void InitServerlist()
    {
        if (haveInitServerlist) return;

        //1，连接服务器 取得游戏服务器列表信息
        //OK
        //2，根据上面的信息 添加服务器列表
        //OK
        for (int i = 0; i < 20; i++)
        {
            string ip = "127.0.0.1:4530";
            GameObject go = null;        
            int count = Random.Range(0, 100);            
            if (count > 50)
            {
                //火爆  
                go = Instantiate(serveritemRed) as GameObject;
                go.GetComponent<ServerProperty>().Name = (i + 1) + "区 图末世界";
            }
            else
            {
                //流畅 
                go = Instantiate(serveritemGreen) as GameObject;
                go.GetComponent<ServerProperty>().Name = (i + 1) + "区 法宝世界";

            }
            go.transform.parent = content.transform;
            go.GetComponent<ServerProperty>().ip = ip;
            //go.GetComponent<ServerProperty>().Name = (i + 1) + "区 图末世界";
            go.GetComponent<ServerProperty>().Count = count;
        }
        haveInitServerlist = true;

    }
    */

    public void HideRegisterImage()
    {
        userRegisterImageDotweenAnimation.DOPlayBackwards();
    }

    public void HideStartImage()
    {
        startChildDotweenAnimation.DOPlayBackwards();
        //roleSelectChildDotweenAnimation.DOPlayForward();
    }
   
    IEnumerator HidePanel(GameObject go)
    {
        yield return new WaitForSeconds(0.3f);
        go.SetActive(false);
    }

    public void ShowStartImage()
    {
        startChildDotweenAnimation.DOPlayForward();
    }
    
 #endregion
    
}
