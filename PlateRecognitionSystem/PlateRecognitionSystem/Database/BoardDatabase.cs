using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateRecognitionSystem.Database
{
    public class BoardDatabase
    {
        private GarageDBContext _dBContext = new GarageDBContext();
        private MainViewModel _viewModel;

        public void AddTableToDatabase(string name, string token) 
        {
            _dBContext.InformationBoards.Add(new Board
            {
                FunctionName = name,
                Token = token
            });
            _dBContext.SaveChanges();
        }

        public void DeleteBoardFromDatabase(string token)
        {
            var board = _dBContext.InformationBoards.SingleOrDefault(x => x.Token == token);
            try
            {
                _dBContext.InformationBoards.Remove(board);
                _dBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public List<Board> GetListOfBoards(string FunctionName)
        {
            var boards = _dBContext.InformationBoards.Where(x => x.FunctionName == FunctionName);
            return boards.ToList();
        }

        public List<Board> GetListOfBoards()
        {   
            return _dBContext.InformationBoards.ToList();
        }
    }
}
