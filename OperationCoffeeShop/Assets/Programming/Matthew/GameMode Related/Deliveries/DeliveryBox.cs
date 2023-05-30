using System.Collections;
using UnityEngine;

public class DeliveryBox : Interactable
{
    public DeliveryData delivery;

    [SerializeField]
    private DeliveryPrefabs _deliveryPrefabs;

    private Grid _grid;

    public override void Start()
    {
        base.Start();
        delivery = gameMode.deliveryManager.GetDelivery();
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        SetupItems();
    }

    private void SetupItems()
    {
        _grid = new Grid(4, 4, .2f,.2f);
        var box=Instantiate(_deliveryPrefabs.openBoxPrefab, transform.position, transform.rotation);
        playerInteraction.Carry(box);
        for(int i = 0; i< delivery.GetDeliveryPackages().Count; i++)
        {
            var gL = _grid.GridLocation(i, transform.position.x-.3f, transform.position.z-.255f);
            var spawnPos = new Vector3(gL.x,transform.position.y-.2f,gL.y);
            switch (delivery.GetDeliveryPackages()[i].objType)
                    {
                        case DeliveryManager.ObjType.Coffee:
                            var obj = Instantiate(_deliveryPrefabs.coffeeDispenserPrefab,box.transform);
                            obj.transform.position = spawnPos;
                            obj.GetComponent<Rigidbody>().isKinematic = true;
                            var disp = obj.GetComponent<Dispenser>();
                            disp.deliveryMode = true;
                            disp.delivered = true;
                            disp.quantity = delivery.GetDeliveryPackages()[i].quantity;
                            break;

                        case DeliveryManager.ObjType.Milk:
                            var obj1 = Instantiate(_deliveryPrefabs.milkDispenserPrefab, box.transform);
                            obj1.transform.position = spawnPos;
                            var iC = obj1.GetComponent<IngredientContainer>();
                            iC.delivered = true;
                            obj1.GetComponent<Rigidbody>().isKinematic = true;
                            break;

                        case DeliveryManager.ObjType.Espresso:
                            var obj2 = Instantiate(_deliveryPrefabs.espressoDispenserPrefab, box.transform);
                            obj2.transform.position = spawnPos;
                            obj2.GetComponent<Rigidbody>().isKinematic = true;
                            var disp1 = obj2.GetComponent<Dispenser>();
                            disp1.deliveryMode = true;
                            disp1.delivered = true;
                            disp1.quantity = delivery.GetDeliveryPackages()[i].quantity;
                            break;

                        case DeliveryManager.ObjType.Sugar:
                            var obj3 = Instantiate(_deliveryPrefabs.sugarDispenserPrefab, box.transform);
                            obj3.transform.position = spawnPos;
                            obj3.GetComponent<Rigidbody>().isKinematic = true;
                            var disp2 = obj3.GetComponent<Dispenser>();
                            disp2.deliveryMode = true;
                            disp2.delivered = true;
                            disp2.quantity = delivery.GetDeliveryPackages()[i].quantity;
                            break;
                        case DeliveryManager.ObjType.Camera:
                            var obj4 = Instantiate(_deliveryPrefabs.cameraPrefab, box.transform);
                            obj4.transform.position = spawnPos;
                            obj4.GetComponent<Rigidbody>().isKinematic = true;
                            obj4.GetComponent<Interactable>().delivered= true;
                            break;
                        case DeliveryManager.ObjType.PictureFrame:
                            var obj5 = Instantiate(_deliveryPrefabs.pictureFramePrefab, box.transform);
                            obj5.transform.position = spawnPos;
                            obj5.GetComponent<Rigidbody>().isKinematic = true;
                            obj5.GetComponent<PictureFrame>().delivered = true;
                            break;
                    }
        }
        Destroy(gameObject);
    }
}