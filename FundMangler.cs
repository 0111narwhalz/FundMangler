using System;
using System.IO;
using System.Collections.Generic;

class FundMangler
{
	const string filePath = "budget.txt";
	const string savePath = "persistent.sfs";
	const string tranPath = "transactions.txt";
	static string[] save;
	
	public static void Main()
	{
		string[] input = new string[1];
		bool userFound = false;
		double[] values;
		List<string> budget = new List<string>();
		if(File.Exists(filePath))
		{
			budget.AddRange(File.ReadAllLines(filePath));
		}
		int userAddress = -1;
		values = new double[3]; 
		
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
				values[0] = double.Parse(Console.ReadLine());
				Console.WriteLine("Input starting science.");
				values[1] = double.Parse(Console.ReadLine());
				Console.WriteLine("Input starting reputation.");
				values[2] = double.Parse(Console.ReadLine());
				
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
				for(int i = FindBlockWithName("SCENARIO", "Funding"); i < save.Length; i++)
				{
					if(save[i].Trim().Split(' ')[0] == "funds")
					{
						values[0] = double.Parse(save[i].Trim().Split(' ')[2]);
						break;
					}
				}
				
				for(int i = FindBlockWithName("SCENARIO", "ResearchAndDevelopment"); i < save.Length; i++)
				{
					if(save[i].Trim().Split(' ')[0] == "sci")
					{
						values[1] = double.Parse(save[i].Trim().Split(' ')[2]);
						break;
					}
				}
				
				for(int i = FindBlockWithName("SCENARIO", "Reputation"); i < save.Length; i++)
				{
					if(save[i].Trim().Split(' ')[0] == "rep")
					{
						values[2] = double.Parse(save[i].Trim().Split(' ')[2]);
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
					values[i] = double.Parse(budget[userAddress].Split(',')[i + 1]);
				}
				
				for(int i = FindBlockWithName("SCENARIO", "Funding"); i < save.Length; i++)
				{
					if(save[i].Trim().Split(' ')[0] == "funds")
					{
						save[i] = "\t\tfunds = " + values[0];
						break;
					}
				}
				
				for(int i = FindBlockWithName("SCENARIO", "ResearchAndDevelopment"); i < save.Length; i++)
				{
					if(save[i].Trim().Split(' ')[0] == "sci")
					{
						save[i] = "\t\tsci = " + values[1];
						break;
					}
				}
				
				for(int i = FindBlockWithName("SCENARIO", "Reputation"); i < save.Length; i++)
				{
					if(save[i].Trim().Split(' ')[0] == "rep")
					{
						save[i] = "\t\trep = " + values[2];
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
		return FindBlock(blockName, 0);
	}
	
	static int FindBlock(string blockName, int start)
	{
		string lastLine;
		string thisLine = "";
		
		for(int i = start; i < save.Length; i++)
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
	
	static int FindBlockWithName(string blockName, string nameValue)
	{
		int index = -1;
		for(int i = 0; i < save.Length; i++)
		{
			i = FindBlock(blockName, i);
			index = i;
			if(save[i + 1].Trim().Split(' ')[0] == "name")
			{
				if(save[i + 1].Trim().Split(' ')[2] == nameValue)
				{
					return i;
				}else
				{
					continue;
				}
			}
		}
		return index;
	}
}











