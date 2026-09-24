using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class DBScript : MonoBehaviour
{
    private string dbName = "URI=file:myDB.db";

    // Start is called before the first frame update
    void Start()
    {
        CreateDB();
        CreatePlayer("Marco", 9999);
        CreatePlayer("John", 3);
        ReadRecords();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //makes a new db (if it doesn't exist) and add a table "players" (if it doesn't exist)
    public void CreateDB()
    {
        //creates a connection object
        using (SqliteConnection connection = new SqliteConnection(dbName)){
            //open the connection
            connection.Open();

            //create a command object
            using (SqliteCommand command = connection.CreateCommand())
            {
                //set the command text
                command.CommandText = "CREATE TABLE IF NOT EXISTS players (name VARCHAR(30), exp INT)";
                //send the command to the DB
                command.ExecuteNonQuery();
            }
        }
    }

    //adds a record for a new player
    public void CreatePlayer(string name, int exp)
    {
        using (SqliteConnection connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                //I find it quite difficult to add a lot of values inside SQL statements
                //so I suggest you do this: first write your statement with some placeholders ({0},{1},...)
                string text = "INSERT INTO players (name, exp) VALUES ('{0}', '{1}');";
                //then combine the string with the values using string.Format
                //name replaces {0}, exp replaces {1}, etc.
                command.CommandText = string.Format(text, name, exp);

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
                command.CommandText = "SELECT * FROM players;";

                //instead of calling ExecuteNonQuery, this time we use ExecuteReader
                using (IDataReader reader = command.ExecuteReader())
                {
                    //while reader still has lines 
                    while (reader.Read())
                    {
                        //we print the values returned from the db.
                        //we can access each field like it was a dictionary
                        Debug.Log("Name: " + reader["name"] + " Exp: " + reader["exp"]);
                    }
                }
            }
        }
    }
}
