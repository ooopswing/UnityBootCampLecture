using UnityEngine;
using TMPro;

public class Calculator : MonoBehaviour
{
    public TMP_Text resultText;

    private string currentInput = "";     // 현재 입력 숫자
    private string operatorSymbol = "";   // + - × ÷
    private float firstValue = 0f;
    private bool hasFirstValue = false;
    private bool isCalculated = false;

    private string expression = "";       // 화면에 표시할 전체 수식
    private string lastNumberPressed = ""; // 마지막으로 누른 숫자
    private const int maxLength = 16;     // 최대 입력 자리수

    // 숫자 버튼
    public void OnClickNumber(string number)
    {
        if (isCalculated)
        {
            ResetAll();
            isCalculated = false;
        }

        // 최대 입력 자리수 체크
        if (expression.Length >= maxLength)
            return;

        if (number == "0")
        {
            // 연속 0 방지: currentInput이 "0"이면 추가하지 않음
            if (currentInput == "0")
                return;

            currentInput += number;
        }
        else
        {
            if (currentInput == "0")
                currentInput = number; // 기존 0 자리 덮어쓰기
            else
                currentInput += number;
        }

        lastNumberPressed = number;
        UpdateExpressionAndDisplay();
    }

    // 연산자 버튼
    public void OnClickOperator(string op)
    {
        if (currentInput == "" && !hasFirstValue) return;

        if (!hasFirstValue)
        {
            float.TryParse(currentInput, out firstValue);
            hasFirstValue = true;
            currentInput = "";
        }
        else if (currentInput != "")
        {
            OnClickEqual(); // 이전 연산 수행
            if (resultText.text == "Error") return;
            float.TryParse(currentInput, out firstValue);
            currentInput = "";
            hasFirstValue = true;
        }

        operatorSymbol = op;
        expression = firstValue.ToString() + operatorSymbol;
        resultText.text = expression;
        lastNumberPressed = "";
    }

    // = 버튼
    public void OnClickEqual()
    {
        if (!hasFirstValue || operatorSymbol == "" || currentInput == "") return;

        float secondValue = 0f;
        float.TryParse(currentInput, out secondValue);

        float result = 0f;
        bool valid = true;

        switch (operatorSymbol)
        {
            case "+": result = firstValue + secondValue; break;
            case "-": result = firstValue - secondValue; break;
            case "×":
            case "*": result = firstValue * secondValue; break;
            case "÷":
            case "/":
                if (Mathf.Approximately(secondValue, 0f))
                    valid = false;
                else
                    result = firstValue / secondValue;
                break;
            default: valid = false; break;
        }

        if (!valid)
        {
            resultText.text = "Error";
            ClearState();
            return;
        }

        // 결과 길이 체크 (16자리 초과 시 계산 중단)
        string resultStr = result.ToString();
        if (resultStr.Length > maxLength)
        {
            resultText.text = "Error";
            ClearState();
            return;
        }

        // 정상 계산
        resultText.text = resultStr;
        currentInput = resultStr;
        operatorSymbol = "";
        hasFirstValue = false;
        expression = "";
        lastNumberPressed = "";
        isCalculated = true;
    }

    // Clear 버튼
    public void OnClickClear()
    {
        ClearState();
        resultText.text = "0";
    }

    // ---------- 도움 함수 ----------
    private void UpdateExpressionAndDisplay()
    {
        if (hasFirstValue && operatorSymbol != "")
            expression = firstValue.ToString() + operatorSymbol + currentInput;
        else
            expression = currentInput;

        resultText.text = string.IsNullOrEmpty(expression) ? "0" : expression;
    }

    private void ClearState()
    {
        currentInput = "";
        operatorSymbol = "";
        firstValue = 0f;
        hasFirstValue = false;
        isCalculated = false;
        expression = "";
        lastNumberPressed = "";
    }

    private void ResetAll()
    {
        currentInput = "";
        operatorSymbol = "";
        firstValue = 0f;
        hasFirstValue = false;
        expression = "";
        lastNumberPressed = "";
        isCalculated = false;
    }
}
