using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using DCN.TicTacToe.UI.SkinComponents;

namespace DCN.TicTacToe.UI.Design
{
    /// <summary>
	/// The class of the designer for the SkinTooltip control.
	/// </summary>
    class SkinCustomAreaDesign : ParentControlDesigner
    {
        #region Fields

        /// <summary>
        /// the filed needed to support hiding of the control in the designer (design time).
        /// </summary>
        private bool isFirstStart = false;

        /// <summary>
        /// the filed defines whether the control is displayed in the designer at the 
        /// given moment.
        /// </summary>
        private bool isControlDisplayed = true;

        /// <summary>
        /// the field saves the location in designer before the DisplayControl property 
        /// is set true.
        /// </summary>
        private Point designTimeLocation;

        /// <summary>
        /// the collection of additional designer's commands.
        /// </summary>
        private DesignerVerbCollection verbs = null;
        /// <summary>
        /// the interface allowing new components to be created in the designer.
        /// </summary>
        private IDesignerHost designerHost = null;
        /// <summary>
        /// the interface allowing the events of changing the selected component to be tracked. 		
        /// </summary>
        private IComponentChangeService componentChangeService = null;
        /// <summary>
        /// the interface alowing the components to be selected.
        /// </summary>
        private ISelectionService selectionService = null;

        /// <summary>
        /// The point where the mouse button was pressed.
        /// </summary>
        private Point pressedPoint = Point.Empty;
        /// <summary>
        /// The label selected in the designer.
        /// </summary>
        private LabelItem selectedItem = null;
        /// <summary>
        /// The point where the mouse button was pressed ( needed to support 
        /// dragging labels with the help of the mouse in the designer )
        /// </summary>
        private bool mousePressed = false;
        /// <summary>
        /// the reference to the control that manages the given class of the designer.
        /// </summary>
        SkinCustomArea skinTooltip;

        #endregion

        #region Constructors

        /// <summary>
        /// The constructorof the designer class. Adds the additional "Add label" command
        /// available to the control in the design-time.
        /// </summary>
        public SkinCustomAreaDesign()
        {
            this.verbs = new DesignerVerbCollection();
            this.verbs.Add(new DesignerVerb("Add label", new EventHandler(this.OnAddLabel)));
        }


        #endregion

        #region Methods

        /// <summary>
        /// the method that helps to add the property only for the design-time to the property grid		
        /// </summary>
        /// <param name="properties">the interface of the dictionary where the needed properties will be added.</param>
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            // Create design-time-only property "DisplayControl" entry and add it to 
            // the  Property Browser's "Design" category.
            properties["DisplayControl"] = TypeDescriptor.CreateProperty(typeof(SkinCustomAreaDesign),
                "DisplayControl", typeof(bool), CategoryAttribute.Design,
                DesignOnlyAttribute.Yes);

        }

        /// <summary>
        /// The function that is called when creating the designer of the component.
        /// </summary>
        /// <param name="component">the reference to the component that will be served by the designer.</param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this.isFirstStart = true;

            this.designerHost = this.GetService(typeof(IDesignerHost)) as
                IDesignerHost;

            this.selectionService = this.GetService(typeof(ISelectionService))
                as ISelectionService;

            this.componentChangeService = this.GetService(typeof
                (IComponentChangeService)) as IComponentChangeService;

            this.Control.MouseDown += new MouseEventHandler(Control_MouseDown);
            this.Control.MouseUp += new MouseEventHandler(Control_MouseUp);
            this.Control.MouseMove += new MouseEventHandler(Control_MouseMove);

            if (this.selectionService != null)
            {
                this.selectionService.SelectionChanged += new EventHandler(selectionService_SelectionChanged);
            }

            if (this.componentChangeService != null)
            {
                this.componentChangeService.ComponentRemoving += new ComponentEventHandler(componentChangeService_ComponentRemoving);
            }

            skinTooltip = this.Control as SkinCustomArea;

            this.DrawGrid = false;
        }

        /// <summary>
        /// The handler method for clicking the "Add label" command in design-time.
        /// </summary>
        /// <param name="sender">The object that invoked the event.</param>
        /// <param name="e">the event's arguments.</param>
        protected void OnAddLabel(object sender, EventArgs e)
        {

            LabelItem item = this.designerHost.CreateComponent(typeof(LabelItem)) as LabelItem;
            item.Parent = skinTooltip;
            skinTooltip.Labels.Add(item);

            item.LabelText = "New label";

            Random rand = new Random();
            item.LabelRect = new Rectangle(rand.Next(skinTooltip.Width - 55), rand.Next(skinTooltip.Height - 15), 55, 15);
        }

        /// <summary>
        /// The handler of clicking the control by mouse.
        /// </summary>
        /// <param name="sender">the object that invoked the event.</param>
        /// <param name="e">the event's parameters.</param>
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {

            foreach (LabelItem item in skinTooltip.Labels)
            {
                if (item.LabelRect.Contains(new Point(e.X, e.Y)))
                {
                    mousePressed = true;

                    UnselectAllItems();

                    this.selectedItem = item;
                    SetSelectComponent(item);

                    this.pressedPoint = new Point(e.X, e.Y);
                    this.selectedItem.Selected = true;
                }
            }

        }

        /// <summary>
        /// Handling the event of releasing the left mouse button.
        /// </summary>
        /// <param name="sender">the object that sent the message.</param>
        /// <param name="e">the message parameters.</param>
        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            mousePressed = false;
        }

        /// <summary>
        /// The function that makes the component selected in the designer.
        /// </summary>
        /// <param name="comp">the component to be selected.</param>
        private void SetSelectComponent(Component comp)
        {
            if (comp == null)
                return;

            Component[] items = new Component[1];
            items[0] = comp;
            if (this.selectionService != null)
                this.selectionService.SetSelectedComponents(items);
        }

        /// <summary>
        /// The function determines whether the mouse click should be handled by the designer.
        /// </summary>
        /// <param name="point">the point that was ckicked.</param>
        /// <returns>returns true if the message should be handled by the designer.</returns>
        protected override bool GetHitTest(Point point)
        {
            Point tpPoint = skinTooltip.PointToClient(point);

            foreach (LabelItem item in skinTooltip.Labels)
                if (item.LabelRect.Contains(tpPoint))
                    return true;

            return false;
        }


        /// <summary>
        /// the handler of the message of mooving the mouse on the control.
        /// </summary>
        /// <param name="sender">the object invoked the event.</param>
        /// <param name="e">the event's parameters.</param>
        private void Control_MouseMove(object sender, MouseEventArgs e)
        {

            Rectangle current;
            Rectangle old;

            if (selectedItem != null && mousePressed)
            {
                current = selectedItem.LabelRect;
                old = current;

                current.X += e.X - pressedPoint.X;
                current.Y += e.Y - pressedPoint.Y;

                selectedItem.LabelRect = current;

                pressedPoint = new Point(e.X, e.Y);
                PropertyDescriptor pd = TypeDescriptor.GetProperties(selectedItem).Find("LabelRect", false);

                componentChangeService.OnComponentChanged(selectedItem, pd, old, current);
            }

        }

        /// <summary>
        /// the handler of the message of changing the selected component.
        /// </summary>
        /// <param name="sender">the object invoked the event.</param>
        /// <param name="e">the event's parameters.</param>
        private void selectionService_SelectionChanged(object sender, EventArgs e)
        {
            LabelItem item = this.selectionService.PrimarySelection as LabelItem;

            if (item != null)
            {
                UnselectAllItems();

                item.Selected = true;
            }

        }

        /// <summary>
        /// The function unselecting all Labels in the SkinTooltip control.
        /// </summary>
        private void UnselectAllItems()
        {
            foreach (LabelItem item in skinTooltip.Labels)
                item.Selected = false;
        }

        /// <summary>
        /// The handler of the message of removing the component from the designer.
        /// It performs the label removal from the control.
        /// </summary>
        /// <param name="sender">the object invoked the event.</param>
        /// <param name="e">the event's parameters</param>
        private void componentChangeService_ComponentRemoving(object sender, ComponentEventArgs e)
        {
            LabelItem item = e.Component as LabelItem;

            if (item != null)
            {
                this.skinTooltip.Labels.Remove(item);
                skinTooltip.Invalidate();
                skinTooltip.Update();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// the property returns the collection of additional designer's commands.
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                return this.verbs;
            }
        }

        /// <summary>
        /// the property defining whether to display SkinTooltip on the form's surface 
        /// in design time.
        /// </summary>
        public bool DisplayControl
        {
            get { return this.isControlDisplayed; }
            set
            {
                if (this.isFirstStart)
                    isControlDisplayed = value;
                else
                    if (isControlDisplayed == value)
                    return;

                if (!value)
                {
                    this.designTimeLocation = this.Control.Location;
                    this.Control.Location = new Point(-this.Control.Width, -this.Control.Height);

                }
                else
                {
                    if (!this.designTimeLocation.IsEmpty)
                        this.Control.Location = this.designTimeLocation;
                    else
                        this.Control.Location = new Point(100, 100);
                }

            }
        }

        #endregion
    }
}
