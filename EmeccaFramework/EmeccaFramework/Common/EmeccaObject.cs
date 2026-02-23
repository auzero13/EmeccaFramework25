
namespace Emecca.Framework.Common
{
    public interface EmeccaObject
    {
        /* 
         * 所有的 EmeccaObject 需定義 系統所屬名稱
         * 定義原則:
         * [SYSTEM].[MODULE].[NAME].[ACTION]
         * 
         * [HDMS].[FRONTDESK].[PROFILE].[EDIT]
         * 
         * 
         **/
        string getEmeccaObjectName();
        string setEmeccaObjectName(string objectId);

        /*
         * Initialized 後會有 EmeccaObjectId
         * */
        string getEmeccaObjectId();
        string setEmeccaObjectId(string objectId);

        /*
         * 定義給系統預先載入時使用
         * */
        //string doInitial();

    }
}
