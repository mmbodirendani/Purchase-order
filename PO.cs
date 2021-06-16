using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using cisseniorproject.purchase;

public partial class Create_Purchase_Order : System.Web.UI.Page
{
    #region Fields

    private List<InventoryPurchaseItem> itemsToOrder;

    #endregion Fields

    #region Methods

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Dictionary<String, PurchaseOrder> purchaseOrders = new Dictionary<String, PurchaseOrder>();

        foreach (RepeaterItem rptItem in OrderItems.Items)
        {

            TextBox orderItemTextBox = (TextBox)rptItem.FindControl("txtAmountToOrder");
            Label itemName = (Label)rptItem.FindControl("lblItemName");
            Label itemPrice = (Label)rptItem.FindControl("lblItemCost");

            PurchaseOrderItem orderItem = new PurchaseOrderItem();
            orderItem.itemName = itemName.Text;
            orderItem.itemPrice = Double.Parse(itemPrice.Text, NumberStyles.Currency);
            orderItem.orderAmount = Convert.ToInt32(orderItemTextBox.Text);

            Label supplier = (Label)rptItem.FindControl("lblSupplier");

          if (!purchaseOrders.ContainsKey(supplier.Text))
          {
              PurchaseOrder newOrder = new PurchaseOrder();

              newOrder.manufacturer.name = supplier.Text;
              newOrder.addItemToOrder(orderItem);

              purchaseOrders[newOrder.manufacturer.name] = newOrder;
          }
          else
          {
              purchaseOrders[supplier.Text].addItemToOrder(orderItem);
          }
          }

        Boolean success = PurchaseManager.processOrder(purchaseOrders.Values.ToList());
        if (success)
        {
            lblMsg.Text = "Purchase orders successfully created.";
        }
        else
        {
            lblMsg.Text = "Sorry, there was an error. Please try again.";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            itemsToOrder = PurchaseManager.getPurchaseOrderItems();

            OrderItems.DataSource = itemsToOrder;
            OrderItems.DataBind();
        }
    }

    #endregion Methods
}
