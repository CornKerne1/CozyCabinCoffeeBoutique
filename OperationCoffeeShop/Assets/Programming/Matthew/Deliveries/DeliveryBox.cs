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
        delivery = gameMode.DeliveryManager.GetDelivery();
    }
    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        SetupItems();
    }

    private void SetupItems()
    {
        _grid = new Grid(4, 4, .2f,.2f);
        var box=Instantiate(_deliveryPrefabs.openBoxPrefab, transform.position, transform.rotation);
        playerInteraction.Carry(box);
        for(int i = 0; i< delivery.GetDeliveryPackages().Count; i++)
        {
            var gL = _grid.GridLocation(i, transform.position.x, transform.position.z);
            var spawnPos = new Vector3(gL.x,transform.position.y,gL.y);
            switch (delivery.GetDeliveryPackages()[i].objType)
                    {
                        case "Coffee":
                            var obj = Instantiate(_deliveryPrefabs.coffeeDispenserPrefab,box.transform);
                            obj.transform.position = spawnPos;
                            obj.GetComponent<Rigidbody>().isKinematic = true;
                            var disp = obj.GetComponent<Dispenser>();
                            disp.deliveryMode = true;
                            disp.GetComponent<Dispenser>().quantity = delivery.GetDeliveryPackages()[i].quantity;
                            break;

                        case "Milk":
                            var obj1 = Instantiate(_deliveryPrefabs.milkDispenserPrefab, box.transform);
                            obj1.transform.position = spawnPos;
                            obj1.GetComponent<Rigidbody>().isKinematic = true;
                            break;

                        case "Espresso":
                            var obj2 = Instantiate(_deliveryPrefabs.espressoDispenserPrefab, box.transform);
                            obj2.transform.position = spawnPos;
                            obj2.GetComponent<Rigidbody>().isKinematic = true;
                            var disp1 = obj2.GetComponent<Dispenser>();
                            disp1.deliveryMode = true;
                            disp1.quantity = delivery.GetDeliveryPackages()[i].quantity;
                            break;

                        case "Sugar":
                            var obj3 = Instantiate(_deliveryPrefabs.sugarDispenserPrefab, box.transform);
                            obj3.transform.position = spawnPos;
                            obj3.GetComponent<Rigidbody>().isKinematic = true;
                            var disp2 = obj3.GetComponent<Dispenser>();
                            disp2.deliveryMode = true;
                            disp2.quantity = delivery.GetDeliveryPackages()[i].quantity;
                            break;
                    }
        }
        Destroy(gameObject);
    }
}