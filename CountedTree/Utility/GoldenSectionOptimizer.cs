using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utility
{
    public class GoldenSectionOptimizer
    {
        public double LowExtreme, HighExtreme;
        public Func<double, double> TheFunction;
        public bool Minimizing;
        public double? Precision;
        public double OptimizationResult;
        public bool NonUnimodalFunctionIsPossible = true; // this should be true only when there may be some noise at relatively high levels of precision. If the broad shape of the function is not unimodal, golden section optimization is inappropriate.

        private int CountHighCandidateChosen = 0, CountLowCandidateChosen = 0;
        private double GetPrecision()
        {
            return Precision ?? (HighExtreme - LowExtreme) / 100.0;
        }
        private double? LowCandidate, HighCandidate;
        private double? LowExtremeFunctionValue, HighExtremeFunctionValue, LowCandidateFunctionValue, HighCandidateFunctionValue;
        private double? OriginalLowExtreme, OriginalHighExtreme;
        private int NonUnimodalErrorsCount;

        public double OptimizeAllowingRangeToExpandIfNecessary()
        {
            double? initialLowExtreme, initialHighExtreme;
            double? result = null;
            int numExpansions = 0;
            const int maxExpansions = 5;
            while (result == null && numExpansions < maxExpansions)
            {
                initialLowExtreme = LowExtreme;
                initialHighExtreme = HighExtreme;
                LowCandidate = null;
                HighCandidate = null;
                numExpansions++;
                result = Optimize();
                // Check whether we need to expand the range
                double maxDeviationDistance = NonUnimodalFunctionIsPossible ? GetPrecision() * 10.0 : 0;
                if (Math.Abs((double)initialHighExtreme - (double)result) <= maxDeviationDistance)
                {
                    HighExtreme = (double)(initialLowExtreme + (initialHighExtreme - initialLowExtreme) * 2.0);
                    LowExtreme = (double)(initialLowExtreme - (initialHighExtreme - initialLowExtreme) * 0.1); // we'll still search some portion of this area
                    result = null;
                }
                else if (Math.Abs((double)initialLowExtreme - (double)result) <= maxDeviationDistance)
                {
                    LowExtreme = (double)(initialHighExtreme - (initialHighExtreme - initialLowExtreme) * 2.0);
                    HighExtreme = (double)(initialHighExtreme + (initialHighExtreme - initialLowExtreme) * 0.1);
                    result = null;
                }
            }
            if (result == null)
                return (HighExtreme + LowExtreme) / 2.0;
            else
                return (double)result;
        }

        public double Optimize()
        {
            if (OriginalHighExtreme == null)
                OriginalHighExtreme = HighExtreme;
            if (OriginalLowExtreme == null)
                OriginalLowExtreme = LowExtreme;
            HighExtremeFunctionValue = LowExtremeFunctionValue = LowCandidateFunctionValue = HighCandidateFunctionValue = null;
            NonUnimodalErrorsCount = 0;
            double precision = GetPrecision();
            bool stopCondition = false;
            double? prematureResult = null; // If this is returned, we have violated our unimodal assumption, and we pick the better of the two interior points; presumably, this should generally occur only when we reach a relatively high level of precision.
            do
            {
                OptimizationRound();
                stopCondition = HighExtreme - LowExtreme < precision;
                if (!stopCondition)
                {
                    prematureResult = NarrowOrPick();
                    if (prematureResult != null)
                        stopCondition = true;
                }
            }
            while (!stopCondition);
            if (prematureResult == null)
            {
                CalculateExtremeFunctionValues();
                OptimizationResult = GetOptimalValueAfterOptimization();
                return OptimizationResult;
            }
            else
                return (double)prematureResult;
        }

        private void OptimizationRound()
        {
            UpdateCandidates();
            CalculateCandidateFunctionValues();
            if (NonUnimodalFunctionIsPossible)
                CalculateExtremeFunctionValues();
        }

        private void UpdateCandidates()
        {
            const double goldenRatio = 0.61803398875;
            double spread = HighExtreme - LowExtreme;
            double goldenLength = spread * goldenRatio;
            if (LowCandidate == null)
                LowCandidate = HighExtreme - goldenLength;
            if (HighCandidate == null)
                HighCandidate = LowExtreme + goldenLength;
        }

        private void CalculateCandidateFunctionValues()
        {
            if (LowCandidate != null && LowCandidateFunctionValue == null)
                LowCandidateFunctionValue = TheFunction((double)LowCandidate);
            if (HighCandidate != null && HighCandidateFunctionValue == null)
                HighCandidateFunctionValue = TheFunction((double)HighCandidate);
        }

        private void CalculateExtremeFunctionValues()
        {
            if (LowExtremeFunctionValue == null)
                LowExtremeFunctionValue = TheFunction((double)LowExtreme);
            if (HighExtremeFunctionValue == null)
                HighExtremeFunctionValue = TheFunction((double)HighExtreme);
        }

        private bool LowCandidateIsMoreOptimalThanHighCandidate()
        {
            if (Minimizing)
                return (double)LowCandidateFunctionValue < (double)HighCandidateFunctionValue;
            else
                return (double)LowCandidateFunctionValue > (double)HighCandidateFunctionValue;
        }

        private bool LowCandidateIsMoreOptimalThanLowExtreme()
        {
            if (Minimizing)
                return (double)LowCandidateFunctionValue < (double)LowExtremeFunctionValue;
            else
                return (double)LowCandidateFunctionValue > (double)LowExtremeFunctionValue;
        }

        private bool HighCandidateIsMoreOptimalThanHighExtreme()
        {
            if (Minimizing)
                return (double)HighCandidateFunctionValue < (double)HighExtremeFunctionValue;
            else
                return (double)HighCandidateFunctionValue > (double)HighExtremeFunctionValue;
        }

        private double GetOptimalValueAfterOptimization()
        {
            bool lowCandidateIsMoreOptimal = LowCandidateIsMoreOptimalThanHighCandidate();
            if (lowCandidateIsMoreOptimal)
            {
                if (LowCandidateIsMoreOptimalThanLowExtreme())
                    return (double)LowCandidate;
                else
                    return (double)LowExtreme;
            }
            else
            {
                if (HighCandidateIsMoreOptimalThanHighExtreme())
                    return (double)HighCandidate;
                else
                    return (double)HighExtreme;
            }
        }

        private bool UnimodalAssumptionViolated()
        {
            if (LowExtremeFunctionValue == null || HighExtremeFunctionValue == null)
                return false;
            bool returnVal = (((LowCandidateFunctionValue > LowExtremeFunctionValue) != (HighCandidateFunctionValue > LowCandidateFunctionValue)) && ((HighExtremeFunctionValue > HighCandidateFunctionValue) != (HighCandidateFunctionValue > LowCandidateFunctionValue)));
            if (returnVal)
                NonUnimodalErrorsCount++;
            return returnVal;
        }

        private double? NarrowOrPick()
        {
            bool considerExtremeValuesWhenNarrowing = false;
            if (UnimodalAssumptionViolated())
            { // return a premature result based on whichever is btter
                if (NonUnimodalErrorsCount >= 4 || CountHighCandidateChosen + CountLowCandidateChosen > 8)
                    return GetOptimalValueAfterOptimization(); // if this is a repeated problem or we've been narrowing for a while, return this even if it is an extreme.
                considerExtremeValuesWhenNarrowing = true;
            }
            Narrow(considerExtremeValuesWhenNarrowing);
            return null; // no premature result
        }

        private void Narrow(bool considerExtremeValuesWhenNarrowing)
        {
            bool lowCandidateIsMoreOptimal;
            if (!considerExtremeValuesWhenNarrowing)
                lowCandidateIsMoreOptimal = LowCandidateIsMoreOptimalThanHighCandidate();
            else
            { // if, e.g., the high extreme is the best, then we'll consider the high candidate more optimal, regardless of whether it's true
                double optimalValue = GetOptimalValueAfterOptimization();
                lowCandidateIsMoreOptimal = (optimalValue == LowExtreme) || (optimalValue == LowCandidate);
            }
            if (lowCandidateIsMoreOptimal)
            {
                CountLowCandidateChosen++;
                HighExtreme = (double)HighCandidate;
                HighExtremeFunctionValue = HighCandidateFunctionValue;
                HighCandidate = LowCandidate;
                HighCandidateFunctionValue = LowCandidateFunctionValue;
                LowCandidate = LowCandidateFunctionValue = null;
            }
            else
            {
                CountHighCandidateChosen++;
                LowExtreme = (double)LowCandidate;
                LowExtremeFunctionValue = LowCandidateFunctionValue;
                LowCandidate = HighCandidate;
                LowCandidateFunctionValue = HighCandidateFunctionValue;
                HighCandidate = HighCandidateFunctionValue = null;
            }
        }

        //// This will only be necessary if the function is not really unimodal.
        //private void Expand()
        //{
        //    const double arbitraryExpansionConstant = 0.7;
        //    double distanceBetweenExtremes = (double)HighExtreme - (double)LowExtreme;
        //    HighExtreme += arbitraryExpansionConstant * distanceBetweenExtremes;
        //    LowExtreme -= arbitraryExpansionConstant * distanceBetweenExtremes;
        //    if (InitialLowExtreme != null && InitialHighExtreme != null)
        //    { // we can't expand beyond original range
        //        if (HighExtreme > InitialHighExtreme)
        //            HighExtreme = (double) InitialHighExtreme;
        //        if (LowExtreme < InitialLowExtreme)
        //            LowExtreme = (double) InitialLowExtreme;
        //    }
        //    HighCandidate = null;
        //    LowCandidate = null;
        //    UpdateCandidates();
        //}

    }

    public static class GoldenSectionOptimizerTester
    {
        public static void DoTest()
        {
            Func<double, double> parab = x => x * x - 2 * x + 1.0;
            GoldenSectionOptimizer opt = new GoldenSectionOptimizer() { LowExtreme = -4.0, HighExtreme = 10.0, Precision = 0.001, Minimizing = true, TheFunction = parab };
            double result = opt.Optimize();
            Debug.WriteLine("Result should be 1.0: " + result);
            opt = new GoldenSectionOptimizer() { LowExtreme = 5.0, HighExtreme = 10.0, Precision = 0.001, Minimizing = true, TheFunction = parab };
            result = opt.Optimize();
            Debug.WriteLine("Result should be 5.0: " + result);
            parab = x => -x * x - 2 * x + 1.0;
            opt = new GoldenSectionOptimizer() { LowExtreme = -10, HighExtreme = 30.0, Precision = 0.001, Minimizing = false, TheFunction = parab };
            result = opt.Optimize();
            Debug.WriteLine("Result should be -1.0: " + result);
            opt = new GoldenSectionOptimizer() { LowExtreme = -10.5, HighExtreme = 11.55, Precision = 0.001, Minimizing = false, TheFunction = parab };
            result = opt.Optimize();
            Debug.WriteLine("Result should be -1.0: " + result);
            opt = new GoldenSectionOptimizer() { LowExtreme = 5.0, HighExtreme = 10.0, Precision = 0.001, Minimizing = false, TheFunction = parab };
            result = opt.Optimize();
            Debug.WriteLine("Result (not allowing range expansion) should be 5.0: " + result);
            opt = new GoldenSectionOptimizer() { LowExtreme = 5.0, HighExtreme = 10.0, Precision = 0.001, Minimizing = false, TheFunction = parab };
            result = opt.OptimizeAllowingRangeToExpandIfNecessary();
            Debug.WriteLine("Result (allowing range expansion) should be -1.0: " + result);
            parab = x => x * x - 2 * x + 1 + Math.Cos(20 * x) + Math.Cos(50.0 * x) / 10.0;
            opt = new GoldenSectionOptimizer() { LowExtreme = -4.0, HighExtreme = 10.0, Precision = 0.00001, Minimizing = true, TheFunction = parab };
            result = opt.Optimize();
            Debug.WriteLine("Result with noisy function should be somewhere between 0.5 and 1.5: " + result);
        }
    }
}
