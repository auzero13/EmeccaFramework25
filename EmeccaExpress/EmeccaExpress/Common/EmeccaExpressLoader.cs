using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Emecca.Framework.Common;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Emecca.Express.Common
{
    /**
     * EmeccaExpressLoader 使用在使用WinForm的專案中執行
     * EmeccaLoader 使用在一般無UI的專案中執行
     * 每個專案應該都需客製化 專案的 Loader 並 override method : public override object doLoad() 
     * 命名方式為 ProjectNameLoader
     */

    public class EmeccaExpressLoader : EmeccaLoader
    {
        public static ILog getLogger()
        {
            return EmeccaLogger.getLogger("EmeccaLogger");
        }

        protected EmeccaSplashScreen frmSplash = null;
        public override object doLoad() 
        {
            int delay = 100;
            
            frmSplash.doReportProgressive("IMSv2012 程式啟動..", 10);
            //getLogger().Info("程式啟動");
            //Thread.Sleep(delay);

            //frmSplash.doReportProgressive("處理系統設定..", 20);
            //getLogger().Info("處理系統設定");
            
            processApplication();
            //Thread.Sleep(delay);
            
            //frmSplash.doReportProgressive("處理系統設定..", 30);
            processDataSource();
            
            //Thread.Sleep(delay);
            //frmSplash.doReportProgressive("處理系統設定..", 40);
            processVariable();
           
            //Thread.Sleep(delay);
            //frmSplash.doReportProgressive("處理資料庫連線設定..", 50);
            
            getLogger().Info("處理資料庫連線設定");
            string assemblyName = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".AssemblyName") as string;
            string strConn = EmeccaRuntime.getInstance().getDataSourceString(assemblyName) as string;
            if (strConn != null)
            {
                // * To Do Extra project dependency code
                //XpoDefault.Session.ConnectionString = strConn;
            }
            else
            {
                getLogger().Error("請設定資料庫連線資訊!!");
                Application.Exit();
            }

            //frmSplash.doReportProgressive("資料庫連線..", 60);
            //getLogger().Info("資料庫連線");
            //Thread.Sleep(delay);
            //frmSplash.doReportProgressive("載入工作項目設定..", 70);
            //getLogger().Info("載入工作項目設定");
            processActions();
            Thread.Sleep(delay);
            //frmSplash.doReportProgressive("系統啟動中..", 80);
            //getLogger().Info("系統啟動中");
            //Thread.Sleep(delay);
            //frmSplash.doReportProgressive("啟動主畫面..", 90);
            //getLogger().Info("啟動主畫面");
           // processMenuBar();
            processToolBar();
            processStatusBar();
            processMenuBar();
            Thread.Sleep(delay);
            //frmSplash.doReportProgressive("系統啟動完成..", 100);
            getLogger().Info("系統啟動完成");
            Thread.Sleep(delay);
            return "PASS";
        }

        public object doExecute()
        {
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.OfficeSkins).Assembly);
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.BonusSkins).Assembly);
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.Skins.SkinManager.EnableMdiFormSkins();

            frmSplash =  new EmeccaSplashScreen();
            object ret = null;
            // TODO : need to move parameter [timeout] and [display cancel button] into system.xml to be configurable
            EmeccaProfile profile = EmeccaRuntime.getInstance().getVariable((EmeccaRuntime.FrameworkReservedVariableLeading + ".EmeccaProfile")) as EmeccaProfile;
            string imgPath = profile.WorkDirectory + "\\RES\\ICONS\\SPLASHSCREEN.JPG";
            Image image = null;
            if (File.Exists(imgPath))
            {
                image = new System.Drawing.Bitmap(imgPath);
            }
           //using( WaitDialogForm frm = new WaitDialogForm("","系統啟動中，請稍後...",300,200))
           //{
           // frm.BackgroundImage = image;
           // frm.Text = "系統啟動中，請等候...";

            using (WaitDialogForm frm = new WaitDialogForm("", "系統啟動中，請等候..."))
            {
                doLoad();
            }
            string version = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".Application.Version") as string;
            string title = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".Application.Title") as string;
            string name = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".Application.Name") as string;

            // force to set main frame title to be system.xml title + version fixed by license control
            EmeccaMainFrame.getInstance().Text = title + "[" + version + "]";

            return ret;
        }

        private Stack tmp = new Stack(); // a tmp stack to process tree structure xml for menu setting 
        private bool isBeginGroup = false; // to process seperator for menu
        public void processToolBar()
        {
            XmlDocument fileXML = EmeccaRuntime.getInstance().getSystemXML();

            /*
             * 載入 XML - <ToolBar> 設定
             * */
            XmlNodeList toolBars = fileXML.SelectNodes("//ToolBar");
            if (toolBars == null)
            {
                getLogger().Error("ToolBar 設定錯誤!!");
            }
            if (toolBars.Count > 0)
            {
                XmlNode toolBar = toolBars[0];

                processBarXml("barTool", toolBar);
            }
            else
            {
                EmeccaMainFrame.getInstance().getBarMgr().Bars["barTool"].Visible = false;
            }
        }

        public void processMenuBar()
        {
            XmlDocument fileXML = EmeccaRuntime.getInstance().getSystemXML();

            /*
            * 載入 XML - <MenuBar> 設定
            * */
            XmlNodeList menuBars = fileXML.SelectNodes("//MenuBar");
            if (menuBars == null)
            {
                getLogger().Error("MenuBar 設定錯誤!!");
            }
            if (menuBars.Count > 0)
            {
                XmlNode menuBar = menuBars[0];

                processBarXml("barMenu", menuBar);
            }
            else
            {
                EmeccaMainFrame.getInstance().getBarMgr().Bars["barMenu"].Visible = false;
            }
        }
        public void processStatusBar()
        {
            XmlDocument fileXML = EmeccaRuntime.getInstance().getSystemXML();

            /*
            * 載入 XML - <MenuBar> 設定
            * */
            XmlNodeList statusBars = fileXML.SelectNodes("//StatusBar");
            if (statusBars == null)
            {
                getLogger().Error("StatusBar 設定錯誤!!");
            }
            if (statusBars.Count > 0)
            {
                XmlNode statusBar = statusBars[0];

                processBarXml("barStatus", statusBar);
            }
            else
            {
                EmeccaMainFrame.getInstance().getBarMgr().Bars["barStatus"].Visible = false;
            }
        }
        private void processBarXml(string barName, XmlNode node)
        {
            if (node == null)
                return;

            LinksInfo linker = null;
            BarItem item = null;
            if (tmp.Count == 0)
            {
                linker = EmeccaMainFrame.getInstance().getBarMgr().Bars[barName].LinksPersistInfo;
            }
            else
            {
                BarSubItem holder = tmp.Peek() as BarSubItem;
                linker = holder.LinksPersistInfo;
            }
            string image = GetAttributeValue(node, "image");
            switch (node.Name)
            {
                case "SubItem":
                    try
                    {
                        string caption = GetAttributeValue(node, "caption");
                        
                        item = new BarSubItem();
                        if (!string.IsNullOrEmpty(image))
                        {
                            if (File.Exists(image))
                            {
                                item.Glyph = Image.FromFile(image);
                            }
                        }
                        item.Manager = EmeccaMainFrame.getInstance().getBarMgr();
                        item.Caption = caption;

                        linker.Add(new LinkPersistInfo(item, isBeginGroup));
                        isBeginGroup = false;
                        tmp.Push(item);
                    }
                    catch (Exception ee)
                    {
                        getLogger().Error("processBarXml-SubItem" + ee, ee);
                    }
                    break;

                case "ButtonItem":
                    try
                    {
                        string caption = GetAttributeValue(node, "caption");
                        string action = GetAttributeValue(node, "action");
                        string tag = GetAttributeValue(node, "resourceId");
                        item = new BarButtonItem();
                        item.Manager = EmeccaMainFrame.getInstance().getBarMgr();
                        item.Caption = caption;
                        item.Visibility = BarItemVisibility.Always;
                        item.Tag = tag;
                        if (!string.IsNullOrEmpty(image))
                        {
                            if (File.Exists(image))
                            {
                                item.Glyph = Image.FromFile(image);
                            }
                        }

                        linker.Add(new LinkPersistInfo(item, isBeginGroup));
                        isBeginGroup = false;

                        EmeccaMainFrame.getInstance().setBarItemAction(item, action);
                    }
                    catch (Exception ee)
                    {
                        getLogger().Error("processBarXml-ButtonItem" + ee);
                    }

                    break;
                case "Seperator":
                    try
                    {
                        isBeginGroup = true;
                    }
                    catch (Exception ee)
                    {
                        
                        getLogger().Error("processBarXml-Seperator" + ee);
                    }
                    break;
                case "LookAndFeel":
                    try
                    {
                        string caption = GetAttributeValue(node, "caption");

                        item = EmeccaMainFrame.getInstance().getLookAndFeelBarItem();
                        item.Manager = EmeccaMainFrame.getInstance().getBarMgr();
                        item.Caption = caption;
                        item.Visibility = BarItemVisibility.Always;
                        if (!string.IsNullOrEmpty(image))
                        {
                            if (File.Exists(image))
                            {
                                item.Glyph = Image.FromFile(image);
                            }
                        }
                        linker.Add(new LinkPersistInfo(item, isBeginGroup));
                        isBeginGroup = false;
                    }
                    catch (Exception ee)
                    {
                        
                        getLogger().Error("processBarXml-LookAndFeel" + ee);
                    }
                    break;
                case "MDIChildren":
                    try
                    {
                        string caption = GetAttributeValue(node, "caption");

                        item = new BarMdiChildrenListItem();
                        item.Manager = EmeccaMainFrame.getInstance().getBarMgr();
                        item.Caption = caption;
                        item.Visibility = BarItemVisibility.Always;
                        if (!string.IsNullOrEmpty(image))
                        {
                            if (File.Exists(image))
                            {
                                item.Glyph = Image.FromFile(image);
                            }
                        }
                        linker.Add(new LinkPersistInfo(item, isBeginGroup));
                        isBeginGroup = false;
                    }
                    catch (Exception ee)
                    {
                        
                        getLogger().Error("processBarXml-MDIChildren" + ee);
                    }
                    break;
                case "CustomBarItem":
                    try
                    {
                        string caption = GetAttributeValue(node, "caption");
                        string clazz = GetAttributeValue(node, "class");
                        string dll = GetAttributeValue(node, "dll");

                        BarItem instance = EmeccaMainFrame.getInstance().createObject(dll, clazz) as BarItem;
                        item = instance;
                        if (!string.IsNullOrEmpty(image))
                        {
                            if (File.Exists(image))
                            {
                                item.Glyph = Image.FromFile(image);
                            }
                        }
                        item.Manager = EmeccaMainFrame.getInstance().getBarMgr();
                        item.Caption = caption;
                        item.Visibility = BarItemVisibility.Always;

                        linker.Add(new LinkPersistInfo(item, isBeginGroup));
                        isBeginGroup = false;
                    }
                    catch (Exception ee)
                    {
                        
                        getLogger().Error("processBarXml-CustomBarItem" + ee);
                    }
                    break;
                //case "ConnectionInfo":
                //    try
                //    {
                //        string caption = GetAttributeValue(node, "caption");

                //        ConnectionInfoItem connInfoItem = new ConnectionInfoItem();
                //        connInfoItem.execute();
                //        item = connInfoItem;

                //        item.Manager = EmeccaMainFrame.getInstance().getBarMgr();
                //        item.Caption = caption;
                //        item.Visibility = BarItemVisibility.Always;

                //        linker.Add(new LinkPersistInfo(item, isBeginGroup));
                //        isBeginGroup = false;
                //    }
                //    catch (EmeccaException ee)
                //    {
                //        processLoaderExecption(ee);
                //    }
                //    break;
                default:
                    break;
            }


            if (item != null)
            {
                item.Appearance.Font = new Font(EmeccaMainFrame.getInstance().getMenuFontName(), EmeccaMainFrame.getInstance().setMenuFontSize());
            }

            //遞歸循環處理子節點
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (!(child.Name.Equals("#text") || child.Name.Equals("#comment")))
                    {
                        processBarXml(barName, child);
                    }
                }
            }

            if ("SubItem".Equals(node.Name))
            {
                tmp.Pop();
            }
        }

        public void processActions()
        {
            XmlDocument fileXML = EmeccaRuntime.getInstance().getSystemXML();

            /*
             * 載入 XML - <Actions> 設定
             * */
            XmlNodeList actions = fileXML.SelectNodes("//Actions");
            if (actions == null || actions.Count < 1)
            {
                
                getLogger().Error("Actions 設定錯誤!!");
            }
            XmlNode action = actions[0];

            processActionXml(action);
        }
        private void processActionXml(XmlNode node)
        {
            if (node == null)
                return;

            switch (node.Name)
            {
                case "Process":
                    try
                    {
                        string key = GetAttributeValue(node, "key");
                        string clazz = GetAttributeValue(node, "class");
                        string preload = GetAttributeValue(node, "preload");
                        string dll = GetAttributeValue(node, "dll");
                        string id = EmeccaRuntime.getInstance().createObjectId();

                        EmeccaProcess instance = EmeccaMainFrame.getInstance().createObject(dll, clazz) as EmeccaProcess;
                        instance.setEmeccaObjectId(id);
                        instance.setEmeccaObjectName(key);
                        EmeccaRuntime.getInstance().setEmeccaObject(key, instance);

                        //instance.doInitial();

                        //Thread workerThread = new Thread(instance.doExecute);
                        //workerThread.Start();

                    }
                    catch (Exception ee)
                    {
                        
                        getLogger().Error("processActionXml 錯誤!!"+ee);

                    }
                    break;
                case "Form":
                    try
                    {
                        string key = GetAttributeValue(node, "key");
                        string clazz = GetAttributeValue(node, "class");
                        string preload = GetAttributeValue(node, "preload");
                        string dll = GetAttributeValue(node, "dll");
                        string type = GetAttributeValue(node, "type");
                        int showMode = 0;
                        string id = EmeccaRuntime.getInstance().createObjectId();
                        if ("true".Equals(preload, StringComparison.CurrentCultureIgnoreCase))
                        {

                            //EmeccaForm instance = EmeccaMainFrame.getInstance().createObject(dll, clazz) as EmeccaForm;
                            // 嘗試建立物件
                            object objInstance = EmeccaMainFrame.getInstance().createObject(dll, clazz);

                            // 使用 Adapter 來處理非 EmeccaForm 的情況
                            EmeccaForm instance = null;
                            if (objInstance is EmeccaForm)
                            {
                                instance = objInstance as EmeccaForm;
                            }
                else if (objInstance is XtraForm)
                {
                    // 自動將 XtraForm 包裝成 EmeccaFormAdapter
                    instance = EmeccaFormAdapter.Adapt(objInstance as XtraForm);
                    getLogger().Info(string.Format("Form '{0}' is not EmeccaForm, auto-adapted using EmeccaFormAdapter", clazz));
                }
                else if (objInstance is Form)
                {
                    // 支援普通的 System.Windows.Forms.Form
                    instance = EmeccaFormAdapter.Adapt(objInstance as Form);
                    getLogger().Info(string.Format("Form '{0}' is standard Form, auto-adapted using EmeccaFormAdapter", clazz));
                }
                else
                {
                    getLogger().Error(string.Format("無法建立物件: {0} 不是 Form 類型", clazz));
                    throw new InvalidCastException(string.Format(
                        "Unable to cast object of type '{0}' to EmeccaForm. " +
                        "Form must inherit from EmeccaForm, XtraForm, or Form.",
                        objInstance != null ? objInstance.GetType().FullName : "null"));
                }

                            instance.setEmeccaObjectId(id);
                            instance.setEmeccaObjectName(key);
                            instance.setEmeccaObjectId(id);
                            instance.setEmeccaObjectName(key);

                            if ("DIALOG".Equals(type))
                            {
                                showMode = 0;
                                instance.setShowMode(EmeccaForm.DIALOG);
                            }
                            if ("MDI".Equals(type))
                            {
                                showMode = 1;
                                instance.setShowMode(EmeccaForm.MDI);
                            }
                            if ("MDI_ONCE".Equals(type))
                            {
                                showMode = 2;
                                instance.setShowMode(EmeccaForm.MDI_ONCE);
                            }
                            if ("WINDOW".Equals(type))
                            {
                                showMode = 3;
                                instance.setShowMode(EmeccaForm.WINDOW);
                            }
                            if ("WINDOW_ONCE".Equals(type))
                            {
                                showMode = 4;
                                instance.setShowMode(EmeccaForm.WINDOW_ONCE);
                            }
                            EmeccaRuntime.getInstance().setEmeccaObject(key, instance);
                        }
                        else
                        {
                            if ("DIALOG".Equals(type))
                            {
                                showMode = 0;
                            }
                            if ("MDI".Equals(type))
                            {
                                showMode = 1;
                            }
                            if ("MDI_ONCE".Equals(type))
                            {
                                showMode = 2;
                            }
                            if ("WINDOW".Equals(type))
                            {
                                showMode = 3;
                            }
                            if ("WINDOW_ONCE".Equals(type))
                            {
                                showMode = 4;
                            }
                            EmeccaRuntime.getInstance().setEmeccaObject(key, null);
                            EmeccaRuntime.getInstance().setVariable(key, dll + "-" + clazz + "-" + showMode);
                        }
                    }
                    catch (Exception ee)
                    {
                        getLogger().Error("processActionXml 錯誤!!" + ee);
                    }
                    break;
                default:
                    break;
            }

            //遞歸循環處理子節點
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (!(child.Name.Equals("#text") || child.Name.Equals("#comment")))
                    {
                        processActionXml(child);
                    }
                }
            }
        }
    }
}
