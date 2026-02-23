
namespace Emecca.Framework.Common
{
    public partial class EmeccaProcess : EmeccaObject
    {
        public string objectId = null;
        public string objectName = null;

        public EmeccaProcess()
        {

        }

        public string getEmeccaObjectName()
        {
            return objectName;
        }
        public string setEmeccaObjectName(string objectName)
        {
            this.objectName = objectName;
            return this.objectName;
        }

        public string getEmeccaObjectId()
        {
            return objectId;
        }
        public string setEmeccaObjectId(string objectId)
        {
            this.objectId = objectId;
            return this.objectId;
        }

    }
}
