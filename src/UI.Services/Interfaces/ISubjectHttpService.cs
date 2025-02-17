﻿using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Services.Models;

namespace UI.Services.Interfaces
{
    public interface ISubjectHttpService
    {
        Task AddSubjectWithGroups(SubjectModel model, string className);
        Task DeleteSubjectWithGroups(int subjectId);
        Task<IEnumerable<SubjectVm>> GetAllSubjectsWithGroups(string className);
    }
}