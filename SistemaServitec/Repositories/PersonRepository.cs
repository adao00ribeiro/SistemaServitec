﻿using Microsoft.EntityFrameworkCore;
using SistemaServitec.Data;
using SistemaServitec.Models;
using SistemaServitec.Repositories.Interfaces;


namespace SistemaServitec.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly SistemaServitecDBContex DbContex;
        public PersonRepository ( SistemaServitecDBContex dbContex )
        {
            DbContex = dbContex;
        }

        public async Task<PersonModel> Add ( PersonModel Person )
        {
            await DbContex.Persons.AddAsync ( Person );
            await  DbContex.SaveChangesAsync ( );
            return Person;
        }

        public async Task<PersonModel> SearchById ( int Id )
        {
            return await DbContex.Persons.Include(x=>x.Endereco).Include ( x => x.Identidade ).FirstOrDefaultAsync ( x => x.Id == Id );
        }

        public async Task<PersonModel> Update ( PersonModel Person , int id )
        {
            PersonModel temp = await SearchById(id);

            if ( temp == null )
            {
                return null;
            }

            DbContex.Persons.Update ( Person );
            await DbContex.SaveChangesAsync ( );
            return Person;
        }

        public async Task<bool> Delete ( int id )
        {
            PersonModel temp = await SearchById(id);

            if ( temp == null )
            {
                throw new Exception ( $"Not {id}" );
            }

            DbContex.Persons.Remove ( temp );
            await DbContex.SaveChangesAsync ( );
            return true;
        }

        public async Task<PersonModel> TakeTheLast ( )
        {
            return await DbContex.Persons.Include ( x => x.Endereco ).Include ( x => x.Identidade ).OrderByDescending ( x=>x.Id).Take(1).SingleAsync();
        }

        public async Task<PersonModel> TakeTheFirst ( )
        {
            return await DbContex.Persons.Include ( x => x.Endereco ).Include ( x => x.Identidade ).FirstAsync ( );
        }

        public async Task<List<PersonModel>> ListAll ( )
        {
            return await DbContex.Persons.ToListAsync ( );
        }
    }
}
