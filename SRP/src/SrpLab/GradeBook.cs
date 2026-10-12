namespace SrpLab;

/// <summary>
/// Grade book: aggregation, letter-band policy, and transcript export.
/// </summary>
public sealed class GradeBook
{
    private readonly Dictionary<string, List<decimal>> _scores = new(StringComparer.OrdinalIgnoreCase);

    public Dictionary<string, List<decimal>> Scores => _scores;

    public void Record(string studentId, decimal score)
    {
        if (score is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(score));
        if (!_scores.TryGetValue(studentId, out var list))
        {
            list = new List<decimal>();
            _scores[studentId] = list;
        }
        list.Add(score);
    }

    public decimal Average(string studentId)
    {
        if (!_scores.TryGetValue(studentId, out var list) || list.Count == 0) return 0m;
        return Math.Round(list.Average(), 2);
    }

    public string Letter(string studentId)
    {
        // Academic policy bands change with faculty senate — not with CSV layout.
        var avg = Average(studentId);
        if (avg >= 90) return "A";
        if (avg >= 80) return "B";
        if (avg >= 70) return "C";
        if (avg >= 60) return "D";
        return "F";
    }

    public bool MeetsHonorRoll(string studentId)
    {
        // Extra academic rule set embedded beside averaging.
        return Average(studentId) >= 85 && Letter(studentId) is "A" or "B";
    }



}


public class PlainTranscript
{
    private readonly GradeBook _gradeBook;
    public PlainTranscript(GradeBook gradeBook)
    {
        _gradeBook = gradeBook;
    }

    public string TranscriptPlain(string studentId, string fullName)
    {
        // Registrar document format ≠ grading policy.
        return $"TRANSCRIPT\nStudent: {fullName} ({studentId})\nAverage: {_gradeBook.Average(studentId)}\nLetter: {_gradeBook.Letter(studentId)}\nHonor: {_gradeBook.MeetsHonorRoll(studentId)}\n";
    }
}




public class SaveToCSV
{
    private readonly GradeBook _gradeBook;
    public SaveToCSV(GradeBook gradeBook)
    {
        _gradeBook = gradeBook;
    }

    public string ExportCsv()
    {
        var rows = new List<string> { "studentId,average,letter,honor" };
        foreach (var id in _gradeBook.Scores.Keys.OrderBy(x => x))
            rows.Add($"{id},{_gradeBook.Average(id)},{_gradeBook.Letter(id)},{(_gradeBook.MeetsHonorRoll(id) ? 1 : 0)}");
        return string.Join('\n', rows);
    }
}

