using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
using XInputDotNetPure;
#endif

// またもや肥大化したのでゲッターとセッター、変数宣言を分割
namespace Player
{
     [System.Serializable]
    public enum PlayerState
    {
        normal = 0,
        buff = 1,
        debuff = 2,
    };
    public partial class PlayerController : MonoBehaviour
    {
        private int _plusPoint;
        private int battlePoint;
        private bool stan; // Safety.csと連動
        private bool isCatch; // Saftey.csと連動
        private bool safetyLock　= true;
        private bool notMove;
        private float throwPower;
        private float raisePower;
        private float accel;
        private float acceleration;
        private float accelLower;
        private float throwLower;
        private int playerNum;
        private float releaseTime; // 強化解除時間
        private float invisible;
        private bool recovery;
        private bool isGround;
        private float stanRelease;
        private float _invincible;
        private int leverCount;
        private int leverMax;
        private float mLostX;
        private float mLostZ;
        private float pLostX;
        private float pLostZ;
        private float _alpha;
        private bool isConfusion;
        private bool runCoroutine;
        private string playerName;
        private PlayerState _state;
        private string padName;
        private Rigidbody rb;
        private Animator _animator;
        private GameController gc;
        private TutoController tc;
        private ObjectController oc;
        private List<Image> _pointImage;
        private BuffIcon _buffIcon;
        private CharStatus status;
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
        private PlayerIndex pIndex;
        private GamePadState gState;
#endif
        [System.NonSerialized] public PlayerController otherPlayer; // Safety.csと連動
        [System.NonSerialized] public GameObject catchObject; // Safety.csと連動
        [System.NonSerialized] public GameObject throwObject;
        [System.NonSerialized] public GameObject throwHasPlayer;
        [SerializeField] GameObject powerHand;
        [SerializeField] List<GameObject> _buff = new List<GameObject>();
        [SerializeField] List<GameObject> _debuff = new List<GameObject>();
        [SerializeField] GameObject _confusion;
        [SerializeField] TextAsset _statusAsset;
        [SerializeField] Material _faceMaterial;
        [SerializeField] Material _bodyMaterial;

        // プレイヤー側でインスペクターからいじれそうに見えるのが嫌なのでprivateとセッター&ゲッターを使用
        public int PlusPoint {
            set { _plusPoint = value; }
            get { return this._plusPoint; }
        }

        public int BattlePoint {
            set { battlePoint = value; }
            get { return this.battlePoint; }
        }

        public bool Stan {
            set { stan = value; }
            get { return this.stan; }
        }

        public bool IsGround {
            set { isGround = value; }
            get { return this.isGround; }
        }

        public bool IsConfusion {
            set { isConfusion = value; }
            get { return this.isConfusion; }
        }

        public bool SafetyLock {
            set { safetyLock = value; }
            get { return this.safetyLock; }
        }

        public bool Recovery {
            set { recovery = value; }
            get { return this.recovery; }
        }

        public float ReleaseTime {
            set { releaseTime = value; }
            get { return this.releaseTime; }
        }

        public float Accel {
            set { accel = value; }
            get { return this.accel; }
        }

        public float Acceleration {
            set { acceleration = value; }
            get { return this.acceleration; }
        }

        public float AccelLower {
            get { return this.accelLower; }
        }

        public float ThrowPower {
            set { throwPower = value; }
            get { return this.throwPower; }
        }

        public float RaisePower {
            set { raisePower = value; }
            get { return this.raisePower; }
        }

        public float ThrowLower {
            get { return this.throwLower; }
        }

        public int LeverMax {
            set { leverMax = value; }
            get { return this.leverMax; }
        }

        public int PlayerNum {
            set { playerNum = value; }
            get { return this.playerNum; }
        }

        public string PlayerName {
            get { return this.playerName; }
        }

        public PlayerState State {
            set { _state = value; }
            get { return _state; }
        }

        public BuffIcon buffIcon {
            get { return _buffIcon; }
            set { _buffIcon = value; }
        }

        public List<GameObject> Buff {
            get { return _buff; }
        }

        public List<GameObject> Debuff {
            get { return _debuff; }
        }

        public GameObject Confusion {
            get { return _confusion; }
        }

        public Animator animator {
            get { return _animator; }
        }
    }
}