using System;
using System.Xml;
using log4net;
using Emecca.Framework.Utility;

namespace Emecca.Framework.Common
{
    public class EmeccaLoader
    {
        public virtual object doLoad()
        {
            processApplication();
            processDataSource();
            processVariable();
            
            string assemblyName = EmeccaRuntime.getInstance().getVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".AssemblyName") as string;
            string strConn = EmeccaRuntime.getInstance().getDataSourceString(assemblyName) as string;
            if (strConn != null)
            {
                // * To Do Extra project dependency code
                //XpoDefault.Session.ConnectionString = strConn;
            }
            else
            {
               EmeccaLogger.getEmeccaLogger().Error("請設定資料庫連線資訊!!");
            }
            
            return "PASS";
        }

        public object doExecute()
        {
            return doLoad();
        }

        public static void processApplication()
        {
            EmeccaLogger.getEmeccaLogger().Info("載入 XML - Application 設定");
            XmlDocument fileXML = EmeccaRuntime.getInstance().getSystemXML();

            EmeccaLogger.getEmeccaLogger().Info("獲取xml中的Application節點");
            XmlNodeList applications = fileXML.SelectNodes("//Application");
            
            if (applications == null || applications.Count < 1 || applications.Count > 1)
            {
                throw new EmeccaException("System.xml Application XmlNode 設定錯誤!");
            }
            XmlNode application = applications[0];
            processApplicationXml(application);
        }
        private static void processApplicationXml(XmlNode node)
        {
            EmeccaLogger.getEmeccaLogger().Info("取得xml中各個節點的訊息");
            EmeccaRuntime.getInstance().setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".Application.Version", GetAttributeValue(node, "version"));
            EmeccaRuntime.getInstance().setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".Application.Title", GetAttributeValue(node, "title"));
            EmeccaRuntime.getInstance().setVariable(EmeccaRuntime.FrameworkReservedVariableLeading + ".Application.Name", GetAttributeValue(node, "name"));
        }

        public static void processDataSource()
        {
            XmlDocument fileXML = EmeccaRuntime.getInstance().getSystemXML();
            XmlNodeList dataSources = fileXML.SelectNodes("//DataSource");
            if (dataSources == null || dataSources.Count < 1)
            {
                throw new EmeccaException("System.xml DataSource XmlNode 設定錯誤!");
            }
            XmlNode dataSource = dataSources[0];
            processDataSourceXml(dataSource);
        }

        private static void processDataSourceXml(XmlNode node)
        {
            if (node == null)
                return;

            switch (node.Name)
            {
                case "Connection":
                    try
                    {
                        string name = GetAttributeValue(node, "name");
                        string _default = GetAttributeValue(node, "default");
                        string info = node.InnerText;
                        //Added by chris 20130712 增加解密处理
                        info = EmeccaEncryptUtility.DecodeDES(info);
                        EmeccaRuntime.getInstance().setDataSourceString(name, info);
                        if ("TRUE".Equals(_default, StringComparison.CurrentCultureIgnoreCase))
                        {
                            EmeccaRuntime.getInstance().setDataSourceString("DEFAULT", info);
                        }
                    }
                    catch (Exception ee)
                    {
                        throw new EmeccaException("System.xml DataSource Connection Node 設定錯誤!" + ee.Message);
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
                        processDataSourceXml(child);
                    }
                }
            }
        }

        public static void processVariable()
        {
            XmlDocument fileXML = EmeccaRuntime.getInstance().getSystemXML();

            XmlNodeList variables = fileXML.SelectNodes("//Variables");
            if (variables == null || variables.Count < 1)
            {
                throw new EmeccaException("System.xml Variables Node 設定錯誤!");
            }
            XmlNode variable = variables[0];

            processVariableXml(variable);
        }
        private static void processVariableXml(XmlNode node)
        {
            if (node == null)
                return;

            switch (node.Name)
            {
                case "Variable":
                    try
                    {
                        string name = GetAttributeValue(node, "name");
                        string value = node.InnerText;
                        EmeccaRuntime.getInstance().setVariable(name, value);
                    }
                    catch (Exception ee)
                    {
                        throw new EmeccaException("System.xml Variable Node 設定錯誤!"+ee.Message);
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
                        processVariableXml(child);
                    }
                }
            }
        }

        protected static string GetAttributeValue(XmlNode node, string name)
        {
            try
            {
                if (node.Attributes[name] == null)
                    return "";
                else
                    return node.Attributes[name].Value;
            }
            catch
            {
                return "";
            }
        }

    }
}
