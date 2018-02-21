using PlateRecognitionSystem.Enums;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateRecognitionSystem.Database
{
    public class BoardDatabase : GarageDBContext
    {
      //  private GarageDBContext _dBContext = new GarageDBContext();
        private MainViewModel _viewModel;

        public void AddTableToDatabase(TypeOfBoards name, string token) 
        {
            InformationBoards.Add(new Board
            {
                FunctionName = name,
                Token = token
            });
            SaveChanges();
        }

        public void DeleteBoardFromDatabase(string token)
        {
            var board = InformationBoards.SingleOrDefault(x => x.Token == token);
            try
            {
                InformationBoards.Remove(board);
                SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public List<Board> GetListOfBoards(TypeOfBoards FunctionName)
        {
            var boards = InformationBoards.Where(x => x.FunctionName == FunctionName);
            return boards.ToList();
        }

        public List<Board> GetListOfBoards()
        {   
            return InformationBoards.ToList();
        }
        public int GetFreeSpaceInGarage()
        {
          
            var garageInformation = GarageInformation.FirstOrDefault();
            if (garageInformation != null)
            {
               var freeSpace =  garageInformation.Capacity - garageInformation.Occupancy;
                if (freeSpace > 0)
                {
                    return freeSpace;
                }
                else
                {
                    return 0; //I scare about minus value ;)
                }
            }
            return 0;
        }
    }
}
