using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Emecca.Framework.Common;
namespace Emecca.Express.Common
{
    public class EmeccaFormAdapter : EmeccaForm
    {
        private Form _wrappedForm;
        private string _objectId;
        private string _objectName;
        private int _showMode = 0;
        public EmeccaFormAdapter(Form form)
        {
            _wrappedForm = form;
            HostWrappedForm();
        }
        public EmeccaFormAdapter()
        {
            _wrappedForm = null;
        }
        public Form WrappedForm
        {
            get { return _wrappedForm; }
        }
        private void HostWrappedForm()
        {
            if (_wrappedForm == null) return;
            _wrappedForm.TopLevel = false;
            _wrappedForm.FormBorderStyle = FormBorderStyle.None;
            _wrappedForm.Dock = DockStyle.Fill;
            this.Text = _wrappedForm.Text;
            this.Size = new System.Drawing.Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(_wrappedForm);
            _wrappedForm.Show();
            _wrappedForm.BringToFront();
            _wrappedForm.FormClosed += (s, e) => this.Close();
        }
        public override string getEmeccaObjectName()
        {
            if (string.IsNullOrEmpty(_objectName))
            {
                _objectName = _wrappedForm != null ? _wrappedForm.GetType().Name : this.GetType().Name;
            }
            return _objectName;
        }
        public override string setEmeccaObjectName(string objectName)
        {
            _objectName = objectName;
            return _objectName;
        }
        public override string getEmeccaObjectId()
        {
            return _objectId;
        }
        public override string setEmeccaObjectId(string objectId)
        {
            _objectId = objectId;
            return _objectId;
        }
        public override int getShowMode()
        {
            return _showMode;
        }
        public override void setShowMode(int mode)
        {
            _showMode = mode;
        }
        public static EmeccaForm Adapt(Form form)
        {
            if (form is EmeccaForm) return form as EmeccaForm;
            if (form is Form) return new EmeccaFormAdapter(form);
            throw new InvalidCastException(string.Format("Form type '{0}' is not supported.", form.GetType().FullName));
        }
        public static EmeccaForm AdaptFromObject(object objInstance)
        {
            if (objInstance == null) return null;
            if (objInstance is EmeccaForm) return objInstance as EmeccaForm;
            if (objInstance is Form) return Adapt(objInstance as Form);
            throw new InvalidCastException(string.Format("Object type '{0}' cannot be adapted.", objInstance.GetType().FullName));
        }
        public static EmeccaForm CreateAndAdapt(Type type)
        {
            if (type == null) return null;
            object instance = Activator.CreateInstance(type);
            return AdaptFromObject(instance);
        }
        public static bool CanAdapt(Type type)
        {
            return typeof(EmeccaForm).IsAssignableFrom(type) ||
                   typeof(XtraForm).IsAssignableFrom(type) ||
                   typeof(Form).IsAssignableFrom(type);
        }
    }
}