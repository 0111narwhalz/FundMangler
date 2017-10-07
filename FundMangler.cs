using System;
using System.IO;
using System.Collections.Generic;

class Budgeter
{
	const string filePath = "budget.txt";
	const string savePath = "persistent.sfs";
	const string tranPath = "transactions.txt";
	static string[] save;
	
	public static void Main()
	{
		string[] input = new string[1];
		bool userFound = false;
		int[] values;
		List<string> budget = new List<string>();
		if(File.Exists(filePath))
		{
			budget.AddRange(File.ReadAllLines(filePath));
		}
		int userAddress = -1;
		values = new int[3]; 
		
		Console.WriteLine("Enter username:");
		string user = Console.ReadLine();
		
		for(int i = 0; i < budget.Count && !userFound; i++)
		{
			input = budget[i].Split(',');
			Console.WriteLine(input[0]);
			if(input[0] == user)
			{
				userFound = true;
				Console.WriteLine("User found.");
				userAddress = i;
				break;
			}
		}
		
		if(!userFound)
		{
			Console.WriteLine("User not found.\nCreate new user? [y/N]");
			if(Console.ReadLine().ToLower() == "y")
			{
				Console.WriteLine("Input starting funds.");
				values[0] = int.Parse(Console.ReadLine());
				Console.WriteLine("Input starting science.");
				values[1] = int.Parse(Console.ReadLine());
				Console.WriteLine("Input starting reputation.");
				values[2] = int.Parse(Console.ReadLine());
				
				budget.Add(String.Format("{0},{1},{2},{3}", user, values[0], values[1], values[2]));
				File.Create(filePath).Dispose();
				File.WriteAllLines(filePath, budget);
				return;
			}
			else
			{
				return;
			}
		}
		
		save = File.ReadAllLines(savePath);
		
		Console.WriteLine("[S]ave, [L]oad, or make a [T]ransaction?");
		switch(Console.ReadLine().ToLower())
		{
			case "s":
				for(int i = FindBlock("CAREER"); i < save.Length; i++)
				{
					input = save[i].Split(' ');
					input[0] = input[0].Trim();
					if(input[0] == "StartingFunds")
					{
						values[0] = int.Parse(input[2]);
						continue;
					}
					if(input[0] == "StartingScience")
					{
						values[1] = int.Parse(input[2]);
						continue;
					}
					if(input[0] == "StartingReputation")
					{
						values[2] = int.Parse(input[2]);
					}
					if(input[0] == "}")
					{
						break;
					}
				}

				for(int i = 0; i < 3; i++)
				{
					try
					{
						Console.WriteLine(values[i]);
					}
					catch(NullReferenceException)
					{
						Console.WriteLine("Not found.");
					}
				}
				
				string data = string.Format("{0},{1},{2},{3}", user, values[0], values[1], values[2]);
				Console.WriteLine(data);
				budget[userAddress] = data;
				File.Create(filePath).Dispose();
				File.WriteAllLines(filePath, budget);
				break;
			
			case "l":
				for(int i = 0; i < 3; i++)
				{
					values[i] = int.Parse(budget[userAddress].Split(',')[i + 1]);
				}
				
				for(int i = FindBlock("CAREER"); i < save.Length; i++)
				{
					input = save[i].Split(' ');
					input[0] = input[0].Trim();
					if(input[0] == "StartingFunds")
					{
						save[i] = "\t\t\tStartingFunds = " + values[0];
						continue;
					}
					if(input[0] == "StartingScience")
					{
						save[i] = "\t\t\tStartingScience = " + values[1];
						continue;
					}
					if(input[0] == "StartingReputation")
					{
						save[i] = "\t\t\tStartingReputation = " + values[2];
					}
					if(input[0] == "}")
					{
						break;
					}
				}
				
				File.Create(savePath).Dispose();
				File.WriteAllLines(savePath, save);
				break;
			
			case "t":
				//TODO: Implement transactions
				Console.WriteLine("Unimplemented.");
				return;
			
			default:
				Console.WriteLine("Unrecognized option.");
				return;
		}
	}
	
	static int FindBlock(string blockName)
	{
		string lastLine;
		string thisLine = "";
		
		for(int i = 0; i < save.Length; i++)
		{
			lastLine = thisLine;
			thisLine = save[i];
			if(thisLine.Trim() == "{" && lastLine.Trim() == blockName)
			{
				return i;
			}
		}
		return -1;
	}
}











