//using System.Diagnostics;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    public Image Icon;
    public string ItemGuid = "";

    public delegate void OnDropItemDelegate(string guid);
    public static event OnDropItemDelegate OnDropItem;

    public InventorySlot()
    {
        //Create a new Image element and add it to the root
        Icon = new Image();
        Add(Icon);

        //Add USS style properties to the elements
        Icon.AddToClassList("SlotIcon");
        AddToClassList("Slot");

        RegisterCallback<PointerDownEvent>(OnPointerDown);
        RegisterCallback<PointerUpEvent>(OnRightClick);
    }

    public void Swap(InventorySlot slot)
    {
        InventorySlot temp = new InventorySlot();
        temp.HoldItem(GameController.GetItemByGuid(this.ItemGuid));

        this.DropItem();
        this.HoldItem(GameController.GetItemByGuid(slot.ItemGuid));

        slot.DropItem();
        slot.HoldItem(GameController.GetItemByGuid(temp.ItemGuid));
    }
    public void HoldItem(ItemDetails item)
    {
        //Debug.Log($"item guid: {item.GUID}");
        Icon.image = item.Icon.texture;
        ItemGuid = item.GUID;
    }

    public void DropItem()
    {
        ItemGuid = "";
        Icon.image = null;
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        //Not the left mouse button
        if (evt.button != 0 || ItemGuid.Equals(""))
        {
            return;
        }

        //Clear the image
        Icon.image = null;

        //Start the drag
        MainUIController.StartDrag(evt.position, this);
    }

    private void OnRightClick(PointerUpEvent evt)
    {
        if (evt.button != 1 || ItemGuid.Equals(""))
            return;

        // @TODO: Remove item from game controller
        OnDropItem.Invoke(ItemGuid);
    }
    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }

    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
