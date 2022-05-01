namespace csharp_ej1
{
    class Language
    {
        private string name;
        private int repoAmmount;
        private double rating;

        public Language(string name, int repoAmmount, double rating)
        {
            this.name = name;
            this.repoAmmount = repoAmmount;
            this.rating = rating;
        }

        public void setRating(double rating)
        {
            this.rating = rating;
        }

        public string getName()
        {
            return this.name;
        }

        public int getRepoAmmount()
        {
            return this.repoAmmount;
        } 

        public double getRating()
        {
            return this.rating;
        }
    }
}