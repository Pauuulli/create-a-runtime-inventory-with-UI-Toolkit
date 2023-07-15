using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIController : MonoBehaviour
{
    public List<InventorySlot> InventoryItems = new();

    private VisualElement m_Root;
    private VisualElement m_SlotContainer;
    private VisualElement m_InventoryContainer;
    
    //Global variable
    private static VisualElement m_GhostIcon;
    private static bool m_IsDragging;
    private static InventorySlot m_OriginalSlot;
    private static VisualElement m_InventoryToggle;
    private static VisualElement m_HPValue;

    private void Awake()
    {
        //Store the root from the UI Document component
        m_Root = GetComponent<UIDocument>().rootVisualElement;

        //Search the root for the SlotContainer Visual Element
        m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");
        m_InventoryContainer = m_Root.Q<VisualElement>("InventoryContainer");

        //Create InventorySlots and add them as children to the SlotContainer
        for (int i = 0; i < 20; i++)
        {
            InventorySlot item = new ();

            InventoryItems.Add(item);

            m_SlotContainer.Add(item);
        }

        GameController.OnInventoryChanged += GameController_OnInventoryChanged;

        m_GhostIcon = m_Root.Query<VisualElement>("GhostIcon");
        m_GhostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        m_GhostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);

        m_InventoryToggle = m_Root.Q<VisualElement>("InventoryToggle");
        m_InventoryToggle.RegisterCallback<ClickEvent>(OnInventoryToggle);

        m_HPValue = m_Root.Q<VisualElement>("HPValue");
    }

    private void GameController_OnInventoryChanged(string[] itemGuid, InventoryChangeType change)
    {
        //Loop through each item and if it has been picked up, add it to the next empty slot
        foreach (string item in itemGuid)
        {
            if (change == InventoryChangeType.Pickup)
            {
                var emptySlot = InventoryItems.FirstOrDefault(x => x.ItemGuid.Equals(""));

                if (emptySlot != null)
                {
                    emptySlot.HoldItem(GameController.GetItemByGuid(item));
                }
            }
        }
    }

    public static void StartDrag(Vector2 position, InventorySlot originalSlot)
    {
        //Set tracking variables
        m_IsDragging = true;
        m_OriginalSlot = originalSlot;

        //Set the new position
        m_GhostIcon.style.top = position.y - m_GhostIcon.layout.height / 2;
        m_GhostIcon.style.left = position.x - m_GhostIcon.layout.width / 2;

        //Set the image
        m_GhostIcon.style.backgroundImage = GameController.GetItemByGuid(originalSlot.ItemGuid).Icon.texture;

        //Flip the visibility on
        m_GhostIcon.style.visibility = Visibility.Visible;
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        //Only take action if the player is dragging an item around the screen
        if (!m_IsDragging)
        {
            return;
        }

        //Set the new position
        m_GhostIcon.style.top = evt.position.y - m_GhostIcon.layout.height / 2;
        m_GhostIcon.style.left = evt.position.x - m_GhostIcon.layout.width / 2;

    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!m_IsDragging)
        {
            return;
        }

        //Check to see if they are dropping the ghost icon over any inventory slots.
        IEnumerable<InventorySlot> slots = InventoryItems.Where(x =>
               x.worldBound.Overlaps(m_GhostIcon.worldBound));

        //Found at least one
        if (slots.Count() != 0)
        {
            InventorySlot closestSlot = slots.OrderBy(x => Vector2.Distance
               (x.worldBound.position, m_GhostIcon.worldBound.position)).First();

            //Set the new inventory slot with the data
            closestSlot.HoldItem(GameController.GetItemByGuid(m_OriginalSlot.ItemGuid));

            //Clear the original slot
            m_OriginalSlot.DropItem();
        }
        //Didn't find any (dragged off the window)
        else
        {
            m_OriginalSlot.Icon.image =
                  GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
        }

        //Clear dragging related visuals and data
        m_IsDragging = false;
        m_OriginalSlot = null;
        m_GhostIcon.style.visibility = Visibility.Hidden;

    }

    private void OnInventoryToggle(ClickEvent evt)
    {
        m_InventoryContainer.style.visibility = (m_InventoryContainer.style.visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
    }

    public static void SetHP(float percent)
    {
        m_HPValue.style.width = Length.Percent(percent);
    }

    
}

