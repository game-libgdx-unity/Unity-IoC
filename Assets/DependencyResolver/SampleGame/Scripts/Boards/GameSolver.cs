﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityIoC;
using Random = System.Random;

namespace App.Scripts.Boards
{

    public class GameSolver : IGameSolver
    {
        [Singleton] private Observable<GameStatus> Status { get; set; }

        [Singleton] private GameBoard Board { get; set; }

        [Singleton] private Random Random { get; set; }
        
        private ICollection<CellData> CellData = Pool<CellData>.List;

        public IEnumerator Solve(float waitForNextStep)
        {
            yield return new WaitForSeconds(waitForNextStep);

            Debug.Log("New Turn");

            while (Status.Value == GameStatus.InProgress)
            {
                if (!CellData.Any(x => x.IsOpened.Value))
                {
                    RandomFirstMove();
                    yield return new WaitForSeconds(waitForNextStep);
                }

                FlagObviousMines();
                yield return new WaitForSeconds(waitForNextStep);

                if (HasAvailableMoves())
                {
                    CalculateNumberOfMines();
                    yield return new WaitForSeconds(waitForNextStep);
                }
                else
                {
                    RandomMove();
                    yield return new WaitForSeconds(waitForNextStep);
                }

                EndTurn_OpenAllCells();
            }

            yield return new WaitForSeconds(waitForNextStep);

            RevealAllMines();

            if (Status.Value == GameStatus.Failed)
            {
                Debug.Log("Solver Failed!");
            }
            else if (Status.Value == GameStatus.Success)
            {
                Debug.Log("Solver Success");
            }
            
            yield return new WaitForSeconds(waitForNextStep);
            Status.Value = GameStatus.Completed;
        }

        private void RevealAllMines()
        {
            Debug.Log("RevealAllMines");

            foreach (var cell in CellData)
            {
                cell.IsOpened.Value = true;
            }
        }

        private void RandomFirstMove()
        {
            Debug.Log("RandomFirstMove");

            var randomX = Random.Next(1, Board.Width - 1);
            var randomY = Random.Next(1, Board.Height - 1);

            Board.FirstMove(randomX, randomY, Random);
            Board.Open(randomX, randomY);
        }

        private void RandomMove()
        {
            Debug.Log("RandomMove");

            var randomID = Random.Next(1, CellData.Count);
            var cell = CellData.First(x => x.ID == randomID);
            while (cell.IsOpened.Value || cell.IsFlagged.Value)
            {
                randomID = Random.Next(1, CellData.Count);
                cell = CellData.First(x => x.ID == randomID);
            }

            Board.Open(cell.X, cell.Y);
        }

        private bool HasAvailableMoves()
        {
            var numberedCells = CellData.Where(x => x.IsOpened.Value && x.AdjacentMines.Value > 0);
            foreach (var numberPanel in numberedCells)
            {
                var neighborCells = Board.GetNeighbors(numberPanel.X, numberPanel.Y);
                var flaggedNeighbors = neighborCells.Where(x => x.IsFlagged.Value);
                if (flaggedNeighbors.Count() == numberPanel.AdjacentMines.Value &&
                    neighborCells.Any(x => !x.IsOpened.Value && !x.IsFlagged.Value))
                {
                    return true;
                }
            }

            return false;
        }

        private void CalculateNumberOfMines()
        {
            Debug.Log("CalculateNumberOfMines");

            var numberedCells = CellData.Where(x => x.IsOpened.Value && x.AdjacentMines.Value > 0);
            foreach (var numberCell in numberedCells)
            {
                var neighborCells = Board.GetNeighbors(numberCell.X, numberCell.Y);

                var flaggedNeighbors = neighborCells.Where(x => x.IsFlagged.Value);

                if (flaggedNeighbors.Count() == numberCell.AdjacentMines.Value)
                {
                    foreach (var hiddenPanel in neighborCells.Where(x => !x.IsOpened.Value && !x.IsFlagged.Value))
                    {
                        Board.Open(hiddenPanel.X, hiddenPanel.Y);
                    }
                }
            }
        }

        private void FlagObviousMines()
        {
            Debug.Log("FlagObviousMines");

            var numberCells = CellData.Where(x => x.IsOpened.Value && x.AdjacentMines.Value > 0);
            foreach (var cell in numberCells)
            {
                var neighborCells = Board.GetNeighbors(cell.X, cell.Y);

                if (neighborCells.Count(x => !x.IsOpened.Value) == cell.AdjacentMines.Value)
                {
                    foreach (var neighbor in neighborCells.Where(x => !x.IsOpened.Value))
                    {
                        Board.Flag(neighbor.X, neighbor.Y);
                    }
                }
            }
        }

        private void EndTurn_OpenAllCells()
        {
            Debug.Log("EndTurn_OpenAllCells");

            var flaggedCells = CellData.Count(x => x.IsFlagged.Value);
            if (flaggedCells == Board.MineCount)
            {
                var hiddenCells = CellData.Where(x => !x.IsFlagged.Value && !x.IsOpened.Value);
                foreach (var cell in hiddenCells)
                {
                    Board.Open(cell.X, cell.Y);
                }
            }
        }
    }
}