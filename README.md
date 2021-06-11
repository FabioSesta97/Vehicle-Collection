# Vehicle-Collection

A simple Console Application that make you Add, Modify and Remove vehicles to/from a list.
The list can be stored on a JSON/XML file to save all your vehicles and load them every time you start the application.

N.B.: 
To save all the changes, you need to edit the following methods:
1) private static async Task<ICollection<Vehicle>> LoadFromJSONAsync(){...}
2) private static async Task SaveToJsonAsync(ICollection<Vehicle> vehicles){...}
3) private static ICollection<Vehicle> LoadFromXML(){...}
4) private static void SaveToXML(ICollection<Vehicle> vehicles){...}
  
In 1) and 2) replace the strings "F:\\ProgettiVS\\Demo2\\Demo2\\Files\\vehicles.json" with "your_JSON_file_PATH".
In 3) and 4) replace the strings "F:\\ProgettiVS\\Demo2\\Demo2\\Files\\vehicles.xml" with "your_XML_file_PATH".

Saving changes is not automatic, you have to do it manually in the MENU!
