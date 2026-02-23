using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.XtraBars;
using Emecca.Framework.Common;
using System.IO;
using System.Drawing;
using Emecca.Framework.DataAccess.Dao;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Xml;
using System.Globalization;
using Emecca.DataAccess.Common;
using System.Threading;
using Emecca.Express.Common.SessionUtils;
using System.Collections.Generic;


using DevExpress.Utils;


namespace Emecca.Express.Common
{
    public partial class EmeccaMainFrame : DevExpress.XtraEditors.XtraForm
    {


        private delegate void InvokeUpdateState(string state);
        public void showLockForm(string state)
        {
            if (this.InvokeRequired)
            {

                this.Invoke(
                    new InvokeUpdateState(this.showLockForm), new Object[] { state }
            );
            }
            else
            {
                EmeccaMainFrame.getInstance().doAction("HDMS.Lock", "");
            }
        }

        public static EmeccaMainFrame instance = new EmeccaMainFrame();
        private static Hashtable assemblyMap = new Hashtable();
        private BarSubItem lnfBarItem = null;
        private Hashtable actions = new Hashtable();
        private Hashtable singletons = new Hashtable();
        float fontSize = 14.0f;
        public string UserInfo = string.Empty;//獲取用戶信息

        bool isEnableGlobalAuth = false;
        private static EmeccaForm showDialogForm = null;

        private EmeccaMainFrame()
        {
            InitializeComponent();
            getFormLocation();
            setMenuFontSize();
          
        }

        private void getFormLocation()
        {
            EmeccaProfile profile = EmeccaRuntime.getInstance().getVariable((EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile")) as EmeccaProfile;

            if (profile != null)
            {
                //多銀幕時，設置DWS顯示的銀幕位置
                Screen[] screen = Screen.AllScreens;

                if (screen.Length >= 2)
                {
                    int index = profile.DisplayScreen;
                    if (index > screen.Length)
                    {
                        index = screen.Length;
                    }

                    //int width = 0;
                    //for (int i = 0; i < index; i++)
                    //{
                    //    //取得設置的第n個顯示的銀幕之前的所有銀幕的寬度之和
                    //    width += screen[i].WorkingArea.Width;
                    //}
                    //this.Left = width;
                    this.Location = new Point(screen[index].Bounds.Left, screen[index].Bounds.Top);  

                }
            }
        }

        public ArrayList getActions()
        {
            ArrayList record = new ArrayList();
            record.AddRange(actions.Values);
            return record;
        }

        public static EmeccaMainFrame getInstance()
        {
            return instance;
        }

        public void doExecute()
        {
            Application.Run(this);
        }

        public void doExecute(EmeccaForm form)
        {
            showDialogForm = form;//一定要寫在前面
            Application.Run(this);
        }


        public void addBarItem(LinksInfo linker, BarItem item)
        {
            linker.Add(new LinkPersistInfo(item, false));
        }

        public BarManager getBarMgr()
        {
            return barMgr;
        }

        public float setMenuFontSize()
        {
            fontSize = 14.0f;
            string enableChangeFont = EmeccaRuntime.getInstance().getVariable("EnableChangeFont") as string;
            if (!string.IsNullOrEmpty(enableChangeFont))
            {
                if (string.Equals(enableChangeFont, "true", StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        fontSize = Convert.ToInt32(Emecca.Framework.Common.EmeccaRuntime.getInstance().getVariable("FontSize"));

                    }
                    catch
                    {
                        fontSize = 14.0f;
                    }
                }
            }

            return fontSize;
        }
        public BarSubItem getLookAndFeelBarItem_NMS()  //2026 steve 切換佈景
        {
            try
            {
                if (lnfBarItem == null)
                {
                    lnfBarItem = new BarSubItem();
                    lnfBarItem.Caption = "切換佈景"; // 建議補上 Caption

                    // 1. 取得 Skin 列表 (不需要轉型 CollectionBase)
                    List<string> skinList = new List<string>();
                    foreach (SkinContainer skin in SkinManager.Default.Skins)
                    {
                        skinList.Add(skin.SkinName);
                    }
                    skinList.Sort();

                    string displayCaption = "";
                    string iconFileName = "";

                    // 假設 EmeccaRuntime 是一個你自定義的類別
                    string systemSkinPath = EmeccaRuntime.getInstance().getVariable("SystemSkin").ToString();

                    // 字型設定
                    Font font = new Font("PMingLiu", fontSize);

                    // 邏輯判斷：如果包含 Skin_Black，則使用自定義對應邏輯
                    if (skinList.Contains("Skin_Black"))
                    {
                        foreach (string skinName in skinList)
                        {
                            // 重置變數
                            displayCaption = "";
                            iconFileName = "";

                            switch (skinName)
                            {
                                case "Skin_Black":
                                    displayCaption = "銀白黑";
                                    iconFileName = "black.ico";
                                    break;
                                case "Skin_Blue":
                                    displayCaption = "海洋藍";
                                    iconFileName = "blue.ico";
                                    break;
                                case "Skin_Caramel":
                                    displayCaption = "咖啡棕";
                                    iconFileName = "caramel.ico";
                                    break;
                                case "Skin_DevExpress Style":
                                    displayCaption = "水晶白";
                                    iconFileName = "expressstyle.ico";
                                    break;
                                case "Skin_iMaginary":
                                    displayCaption = "奇幻藍";
                                    iconFileName = "imaginary.ico";
                                    break;
                                case "Skin_Lilian":
                                    displayCaption = "紫丁香";
                                    iconFileName = "lilian.ico";
                                    break;
                                case "Skin_Money Twins":
                                    displayCaption = "優雅藍";
                                    iconFileName = "moneytwins.ico";
                                    break;
                                default:
                                    continue; // 如果不在列表中則跳過
                            }

                            // 檢查圖示是否存在
                            if (File.Exists(systemSkinPath + iconFileName))
                            {
                                BarCheckItem item = new BarCheckItem(barMgr, false);
                                item.Caption = displayCaption;
                                item.Appearance.Font = font;
                                item.Tag = skinName; // Tag 存原始 Skin 名稱
                                item.Glyph = Image.FromFile(systemSkinPath + iconFileName);

                                // 判斷是否為當前選中的 Skin
                                // lnfMain 是 DefaultLookAndFeel 元件
                                item.Checked = item.Tag.ToString().Equals(lnfMain.LookAndFeel.SkinName);

                                item.ItemClick += new ItemClickEventHandler(onSwitchSkin);
                                lnfBarItem.ItemLinks.Add(item);
                            }
                        }
                    }
                    else
                    {
                        // 如果沒有 Skin_Black (例如沒有註冊 BonusSkins)，則列出所有找到的 Skin
                        foreach (string skinName in skinList)
                        {
                            BarCheckItem item = new BarCheckItem(barMgr, false);
                            item.Caption = skinName;
                            item.Appearance.Font = font;
                            item.Tag = skinName;

                            item.Checked = item.Caption.Equals(lnfMain.LookAndFeel.SkinName);

                            item.ItemClick += new ItemClickEventHandler(onSwitchSkin);
                            lnfBarItem.ItemLinks.Add(item);

                            // Console.WriteLine(item.Tag.ToString()); // 除錯用
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EmeccaLogger.getEmeccaLogger().Error("getLookAndFeelBarItem " + ex.Message, ex);
                // MessageBox.Show 改為標準調用或保留你們的封裝
                EmeccaMessageBox.Show("切換佈景主題發生錯誤:" + ex.Message, "承啓醫系科室管理系統");
            }
            return lnfBarItem;
        }

        public BarSubItem getLookAndFeelBarItem()
        {
            try
            {
                if (lnfBarItem == null)
                {
                    lnfBarItem = new BarSubItem();
                    ArrayList arr = new ArrayList();
                    foreach (SkinContainer cnt in SkinManager.Default.Skins) 
                    {
                        arr.Add(cnt.SkinName);
                    }
                    arr.Sort();
                    string strSkinName = "";
                    string strSkinPath = "";
                    string strSkinGlyph = EmeccaRuntime.getInstance().getVariable("SystemSkin").ToString();
                    Font font = new Font("PMingLiu", fontSize);
                    if (arr.Contains("Skin_Black"))
                    {
                        if (!string.IsNullOrWhiteSpace(strSkinGlyph) && !strSkinGlyph.StartsWith("Skin_"))
                        {
                            //strSkinGlyph = "Skin_" + strSkinGlyph;
                        }
                        foreach (string name in arr)
                        {
                            switch (name)
                            {
                                case "Skin_Black":
                                    strSkinName = "銀白黑";
                                    strSkinPath = "black.ico";
                                    break;
                                case "Skin_Blue":
                                    strSkinName = "海洋藍";
                                    strSkinPath = "blue.ico";
                                    break;
                                case "Skin_Caramel":
                                    strSkinName = "咖啡棕";
                                    strSkinPath = "caramel.ico";
                                    break;
                                ////Modified by chris 20121018 取消深邃黑
                                //case "DevExpress Dark Style":
                                //    strSkinName = "深邃黑";
                                //    strSkinPath = "expressdarkstyle.ico";
                                ///   break;
                                case "Skin_DevExpress Style":
                                    strSkinName = "水晶白";
                                    strSkinPath = "expressstyle.ico";
                                    break;
                                case "Skin_iMaginary":
                                    strSkinName = "奇幻藍";
                                    strSkinPath = "imaginary.ico";
                                    break;
                                case "Skin_Lilian":
                                    strSkinName = "紫丁香";
                                    strSkinPath = "lilian.ico";
                                    break;
                                case "Skin_Money Twins":
                                    strSkinName = "優雅藍";
                                    strSkinPath = "moneytwins.ico";
                                    break;
                                default:
                                    continue;
                            }
                            if (File.Exists(strSkinGlyph + strSkinPath))
                            {
                                BarCheckItem subItem = new BarCheckItem(barMgr, false);
                                subItem.Caption = strSkinName;
                                subItem.Appearance.Font = font;
                                subItem.Tag = name;
                                subItem.Glyph = Image.FromFile(strSkinGlyph + strSkinPath);
                                subItem.Checked = subItem.Caption.Equals(lnfMain.LookAndFeel.SkinName);
                                subItem.ItemClick += onSwitchSkin;
                                lnfBarItem.ItemLinks.Add(subItem);
                            }
                        }
                    }
                    else
                    {
                        foreach (string name in arr)
                        {
                            switch (name)
                            {
                                case "Black":
                                    strSkinName = "銀白黑";
                                    strSkinPath = "black.ico";
                                    break;
                                case "Blue":
                                    strSkinName = "海洋藍";
                                    strSkinPath = "blue.ico";
                                    break;
                                case "Caramel":
                                    strSkinName = "咖啡棕";
                                    strSkinPath = "caramel.ico";
                                    break;
                                ////Modified by chris 20121018 取消深邃黑
                                //case "DevExpress Dark Style":
                                //    strSkinName = "深邃黑";
                                //    strSkinPath = "expressdarkstyle.ico";
                                ///   break;
                                case "DevExpress Style":
                                    strSkinName = "水晶白";
                                    strSkinPath = "expressstyle.ico";
                                    break;
                                case "iMaginary":
                                    strSkinName = "奇幻藍";
                                    strSkinPath = "imaginary.ico";
                                    break;
                                case "Lilian":
                                    strSkinName = "紫丁香";
                                    strSkinPath = "lilian.ico";
                                    break;
                                case "Money Twins":
                                    strSkinName = "優雅藍";
                                    strSkinPath = "moneytwins.ico";
                                    break;
                                default:
                                    continue;
                            }
                            if (File.Exists(strSkinGlyph + strSkinPath))
                            {
                                BarCheckItem subItem = new BarCheckItem(barMgr, false);
                                subItem.Caption = strSkinName;
                                subItem.Appearance.Font = font;
                                subItem.Tag = name;
                                subItem.Glyph = Image.FromFile(strSkinGlyph + strSkinPath);
                                subItem.Checked = subItem.Caption.Equals(lnfMain.LookAndFeel.SkinName);
                                subItem.ItemClick += onSwitchSkin;
                                lnfBarItem.ItemLinks.Add(subItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error("getLookAndFeelBarItem " + ee.Message, ee);
                EmeccaMessageBox.Show("切換佈景主題發生錯誤:" + ee.Message, "承啓醫系科室管理系統");
            }

            return lnfBarItem;
        }

        public void setBarItemAction(BarItem item, string action)
        {
            actions.Add(item, action);
            item.ItemClick += onSelectItem;
        }


        public EmeccaForm getEmeccaForm(string action)
        {
            EmeccaForm instance = EmeccaRuntime.getInstance().getEmeccaObject(action) as EmeccaForm;
            //Added by chris 20140709 修改增加preload
            if (instance == null)
            {
                string keyAndClazz = EmeccaRuntime.getInstance().getVariable(action) as string;
                string[] strings = keyAndClazz.Split('-');
                string dll = strings[0];
                string clazz = strings[1];
                int showMode = int.Parse(strings[2]);
                if (string.IsNullOrWhiteSpace(dll))
                {
                    instance = EmeccaFormAdapter.AdaptFromObject(createObject(clazz));
                }
                else
                {
                    instance = EmeccaFormAdapter.AdaptFromObject(createObject(dll, clazz));
                }
                string id = EmeccaRuntime.getInstance().createObjectId();
                instance.setShowMode(showMode);
                instance.setEmeccaObjectId(id);
                instance.setEmeccaObjectName(action);
                EmeccaRuntime.getInstance().setEmeccaObject(action, instance);
            }
            EmeccaForm newFrm = null;
            if (typeof(EmeccaForm).IsInstanceOfType(instance))
            {
                try
                {
                    Type clazz = instance.GetType();
                    EmeccaForm frm = instance as EmeccaForm;

                    int mode = frm.getShowMode();

                    switch (mode)
                    {
                        case EmeccaForm.DIALOG:
                            newFrm = EmeccaFormAdapter.CreateAndAdapt(clazz);
                            newFrm.setEmeccaObjectName(frm.getEmeccaObjectName());
                            newFrm.setEmeccaObjectId(EmeccaRuntime.getInstance().createObjectId());
                            break;
                        case EmeccaForm.MDI:
                            newFrm = EmeccaFormAdapter.CreateAndAdapt(clazz);
                            newFrm.setEmeccaObjectName(frm.getEmeccaObjectName());
                            newFrm.setEmeccaObjectId(EmeccaRuntime.getInstance().createObjectId());
                            newFrm.setShowMode(frm.getShowMode());
                            break;
                        case EmeccaForm.MDI_ONCE:
                            if (!Contains(frm))
                            {
                                newFrm = EmeccaFormAdapter.CreateAndAdapt(clazz);
                                newFrm.setEmeccaObjectName(frm.getEmeccaObjectName());
                                newFrm.setEmeccaObjectId(EmeccaRuntime.getInstance().createObjectId());
                                newFrm.setShowMode(frm.getShowMode());
                                EmeccaRuntime.getInstance().setEmeccaObject(action, newFrm);
                            }
                            else
                            {
                                newFrm = frm;
                            }
                            break;
                        case EmeccaForm.WINDOW:
                            newFrm = EmeccaFormAdapter.CreateAndAdapt(clazz);
                            newFrm.setEmeccaObjectName(frm.getEmeccaObjectName());
                            newFrm.setEmeccaObjectId(EmeccaRuntime.getInstance().createObjectId());
                            newFrm.setShowMode(frm.getShowMode());
                            break;
                        case EmeccaForm.WINDOW_ONCE:
                            if (!Contains(frm))
                            {
                                newFrm = EmeccaFormAdapter.CreateAndAdapt(clazz);
                                newFrm.setEmeccaObjectName(frm.getEmeccaObjectName());
                                newFrm.setEmeccaObjectId(EmeccaRuntime.getInstance().createObjectId());
                                newFrm.setShowMode(frm.getShowMode());
                                EmeccaRuntime.getInstance().setEmeccaObject(action, newFrm);
                            }
                            else
                            {
                                newFrm = frm;
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ee)
                {
                    EmeccaLogger.getEmeccaLogger().Error("getEmeccaForm " + ee.Message, ee);
                    EmeccaMessageBox.Show("初始化視窗錯誤:" + ee.Message, "承啓醫系科室管理系統");
                }
            }

            return newFrm;

        }

        public void showEmeccaForm(EmeccaForm frm)
        {
            showEmeccaForm(frm, "");
        }

        public void showEmeccaForm(EmeccaForm frm, string caption)
        {
            int mode = frm.getShowMode();

            EmeccaForm activeFrm = singletons[frm.getEmeccaObjectName()] as EmeccaForm;
            if (activeFrm == null)
            {
                singletons.Add(frm.getEmeccaObjectName(), frm);
            }
            switch (mode)
            {
                case EmeccaForm.DIALOG:
                    frm.ShowDialog();
                    break;
                case EmeccaForm.MDI:
                    frm.MdiParent = this;
                    frm.Text = caption;
                    frm.Show();
                    break;
                case EmeccaForm.MDI_ONCE:
                    frm.MdiParent = this;
                    frm.Text = caption;
                    frm.Show();
                    frm.Focus();
                    break;
                case EmeccaForm.WINDOW:
                    frm.Show();
                    frm.Text = caption;
                    frm.Focus();
                    break;
                case EmeccaForm.WINDOW_ONCE:
                    activeFrm.Show();
                    activeFrm.Text = caption;
                    activeFrm.Focus();
                    break;
                default:
                    break;
            }
        }

        public void doAction(string action, string caption)
        {
            try
            {
                EmeccaObject instance = getEmeccaForm(action);
               
                if (typeof(EmeccaForm).IsInstanceOfType(instance))
                {
                    EmeccaForm frm = instance as EmeccaForm;
                    showEmeccaForm(frm, caption);
                }
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error("doAction " + ee.Message, ee);
                EmeccaMessageBox.Show("初始化視窗作業錯誤:" + ee.Message, "承啓醫系科室管理系統");
            }
        }
        private void onSelectItem(object sender, ItemClickEventArgs e)
        {
            BarItem item = e.Item as BarItem;
            string action = actions[item] as string;
            if (isEnableGlobalAuth)
            {
                if (!ResourcesBasDao.Instance.getUserPermissions(item.Tag))
                {
                    return;
                }
            }
            doAction(action, item.Caption);
        }

        private void onSwitchSkin(object sender, ItemClickEventArgs e)
        {
            Application.DoEvents();
            BarCheckItem item = e.Item as BarCheckItem;
            try
            {
                lnfMain.LookAndFeel.SkinName = item.Tag.ToString();
                EmeccaProfile profile = EmeccaRuntime.getInstance().getVariable((EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile")) as EmeccaProfile;
                profile.LookAndFeel = lnfMain.LookAndFeel.SkinName;
                EmeccaRuntime.getInstance().doSerialize(profile);
                this.Refresh();

                foreach (BarCheckItemLink option_item_link in lnfBarItem.ItemLinks)
                {
                    BarCheckItem option_item = option_item_link.Item as BarCheckItem;
                    option_item.Checked = option_item.Caption.Equals(lnfMain.LookAndFeel.SkinName);
                }
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error("onSwitchSkin:" + ee,ee);
                //EmeccaMessageBox.Show("切換視窗佈景錯誤:" + ee.Message, "承啓醫系科室管理系統");
            }
        }

public Object createObject(string clazz)
        {
            Object instance = null;
            try
            {
                string defaultAssemblyName = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".AssemblyName") as string;
                if (!assemblies.ContainsKey(defaultAssemblyName))
                {
                    Assembly assembly = Assembly.Load(defaultAssemblyName);
                    instance = assembly.CreateInstance(clazz);
                    assemblies.Add(defaultAssemblyName, assembly);
                }
                else
                {
                    instance = assemblies[defaultAssemblyName].CreateInstance(clazz);
                }
   
                //Type type = Type.GetType(clazz);
                //instance = Activator.CreateInstance(type) as EmeccaObject;
                //if (type == null)
                //    return null;

                //instance = Activator.CreateInstance(defaultAssemblyName, clazz) as EmeccaObject;
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error("createObject(string clazz) " + ee.Message, ee);
                EmeccaMessageBox.Show("建立物件錯誤:" + ee.Message, "承啓醫系科室管理系統");
            }
            return instance;
        }

        Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

        public Object createObject(string dll, string clazz)
        {
            Object instance = null;
            try
            {
                if (dll == null || "".Equals(dll.Trim()))
                {
                    return createObject(clazz);
                }
                else
                {
                    string defaultAssemblyName = dll.Split('.')[0];
                    if (!assemblies.ContainsKey(defaultAssemblyName))
                    {
                        Assembly assembly = Assembly.Load(defaultAssemblyName);
                        instance = assembly.CreateInstance(clazz);
                        assemblies.Add(defaultAssemblyName, assembly);
                    }
                    else
                    {
                        instance = assemblies[defaultAssemblyName].CreateInstance(clazz);
                    }
                    return instance;
                }
                /*Marked By alan 20120525 Unreachable Code
                Assembly lib = assemblyMap[dll] as Assembly;
                if (lib == null)
                {
                    lib = Assembly.LoadFile(dll);
                    assemblyMap.Add(dll, lib);
                    if (lib == null)
                        return null;
                }
                Type type = lib.GetType(clazz);
                if (type == null)
                    return null;
                instance = Activator.CreateInstance(type);
                */
            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error("createObject(string dll, string clazz) " + ee.Message, ee);
                EmeccaMessageBox.Show("建立物件錯誤:" + ee.Message, "承啓醫系科室管理系統");
            }
            return instance;
        }

        private void EmeccaMainFrame_Load(object sender, EventArgs e)
        {
            try
            {
                string strEnableGlobalAuth = EmeccaRuntime.getInstance().getVariable("EnableGlobalAuth") as string;
                if (strEnableGlobalAuth != null)
                {
                    if (!"FALSE".Equals(strEnableGlobalAuth, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isEnableGlobalAuth = true;
                    }
                }
                bool customSkin = false;
                if (lnfBarItem.ItemLinks.Count > 0)
                {
                    BarCheckItem chkItem = lnfBarItem.ItemLinks[0].Item as BarCheckItem;
                  string tag=  chkItem.Tag as string;
                  if (!string.IsNullOrWhiteSpace(tag) && tag.StartsWith("Skin_"))
                  {
                      customSkin = true;
                  }
                }
                //載入儲存的佈景
                EmeccaProfile profile = EmeccaRuntime.getInstance().getVariable((EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile")) as EmeccaProfile;
                if (customSkin &&!string.IsNullOrWhiteSpace(profile.LookAndFeel) && !profile.LookAndFeel.StartsWith("Skin_"))
                {
                    lnfMain.LookAndFeel.SkinName = "Skin_" + profile.LookAndFeel;
                }
                else
                {
                    lnfMain.LookAndFeel.SkinName =  profile.LookAndFeel;
                }
                Refresh();

                this.barUserInfo.Caption = UserInfo;

                foreach (BarCheckItemLink option_item_link in lnfBarItem.ItemLinks)
                {
                    BarCheckItem option_item = option_item_link.Item as BarCheckItem;
                    option_item.Checked = option_item.Caption.Equals(lnfMain.LookAndFeel.SkinName);
                }

                Font font = new System.Drawing.Font("PMingLiu", fontSize);
                tmdiLayoutMain.AppearancePage.Header.Font = font;
                tmdiLayoutMain.AppearancePage.HeaderActive.Font = font;
                tmdiLayoutMain.AppearancePage.HeaderHotTracked.Font = font;
                if (showDialogForm != null)
                {
                    showDialogForm.ShowDialog();
                }

            }
            catch (Exception ee)
            {
                EmeccaLogger.getEmeccaLogger().Error("EmeccaMainFrame_Load " + ee.Message, ee);
                EmeccaMessageBox.Show("初始化視窗發生錯誤:"+ee.Message, "承啓醫系科室管理系統");
            }
        }

        private void saveFormLocation()
        {
            int displayScreen = 0;
            int formLeft = this.Left;
            try
            {
                EmeccaProfile profile = EmeccaRuntime.getInstance().getVariable((EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile")) as EmeccaProfile;
                if (profile != null)
                {
                    //多銀幕時，設置DWS顯示的銀幕位置
                    Screen[] screen = Screen.AllScreens;

                    for (int i = 0; i < screen.Length; i++)
                    {
                        if (formLeft < screen[i].WorkingArea.Right)
                        {
                            displayScreen = i + 1;
                            break;
                        }
                    }

                    if (profile.DisplayScreen != displayScreen)
                    {
                        profile.DisplayScreen = displayScreen;
                        EmeccaRuntime.getInstance().setEmeccaProfile(profile);
                    }
                }
            }
            catch (Exception ex)
            {
                EmeccaLogger.getEmeccaLogger().Error("saveFormLocation " + ex.Message,ex);
                EmeccaMessageBox.Show("紀錄視窗位置錯誤:" + ex.Message, "承啓醫系科室管理系統");
            }
        }

        private void EmeccaMainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //saveFormLocation();
            DialogResult dialRe = EmeccaMessageBox.Show("確定要離開系統?");
            if (dialRe == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            this.barDateTime.Caption = string.Format("時間:{0}", date.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private void EmeccaMainFrame_Activated(object sender, EventArgs e)
        {
            //20130109 marked by chris 看一下是不是SessionTimeOut引發當機的
            //SessionControlEventUtil sessionEvent = new SessionControlEventUtil();
            //sessionEvent.setTimeOut(this);
        }

        private void EmeccaMainFrame_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // Use this since we are a WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Use this since we are a console app
                System.Environment.Exit(1);
            }
        }
    }
}