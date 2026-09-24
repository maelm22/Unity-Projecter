using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.Security.Cryptography;

public class Shopkeeper : MonoBehaviour
{
    // a reference to the canvas, so you can manipulate it
    public GameObject shopCanvas;

    // the name of the shopkeeper
    public string shopkeeperName = "Aaron";

    // the list of items the shopkeeper has in stock (which you should retrieve/reconstruct from the db 
    public List<Item> items;
    
    private string dbName = "URI=file:myDB.db";

    // Start is called before the first frame update
    void Start()
    {
        CreateShop("sword", 100);
        CreateShop("shield", 90);
        CreateShop("armor", 150);
        CreateShop("book", 50);
        CreateShop("spear", 100);
        CreateShop("shoe", 200);
        CreateShop("axe", 125);
        CreateShop("greatSword", 130);
        ReadRecords();
    }
    
    
    
    public void CreateShop(string name, int price)
    {
        using (SqliteConnection connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                //I find it quite difficult to add a lot of values inside SQL statements
                //so I suggest you do this: first write your statement with some placeholders ({0},{1},...)
                string text = $"INSERT INTO shopkeper (ShopKeeperName, name, price) VALUES (shopkeeperName, names, price);";
                //then combine the string with the values using string.Format
                //name replaces {0}, exp replaces {1}, etc.
                command.CommandText = string.Format(text, name, price);
                
                command.ExecuteNonQuery();
            }
        }
    }

    public void ReadRecords()
    {
        using (SqliteConnection connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                //this time we're asking to select all thee rows in the players table
                command.CommandText = "SELECT * FROM shopkeper;";

                //instead of calling ExecuteNonQuery, this time we use ExecuteReader
                using (IDataReader reader = command.ExecuteReader())
                {
                    //while reader still has lines 
                    while (reader.Read())
                    {
                        //we print the values returned from the db.
                        //we can access each field like it was a dictionary
                        Debug.Log("Name: " + reader["name"] + " Price: " + reader["price"]);
                        
                        Item item = new Item(reader["name"].ToString(), (int)reader["price"]);
                        this.items.Add(item);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Method called when the shop is activated by the player, here you want to 
    /// 1 activate the shop canvas
    /// 2 get the items from the db
    /// 3 display the info on the shop canvas
    /// </summary>
    public void ActivateShop()
    {
        shopCanvas.SetActive(true);

        

        

    }

    /// <summary>
    /// Deactivating the shop is easier, we can just deactivate the shop canvas.
    /// </summary>
    public void DeactivateShop()
    {
        shopCanvas.SetActive(false);
    }

}
