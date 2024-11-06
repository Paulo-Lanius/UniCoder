namespace UniCoder.Services
{
    public class HuffmanNode
    {
        public char? Character { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode? Left { get; set; }
        public HuffmanNode? Right { get; set; }

        public bool IsLeaf => Left == null && Right == null;

        // Sobrescreve o método ToString para facilitar a exibição do nó
        public override string ToString()
        {
            if (IsLeaf && Character.HasValue)
            {
                return $"Leaf: '{Character}' - Freq: {Frequency}";
            }
            else
            {
                return $"Node: Freq: {Frequency}";
            }
        }
    }

    public class HuffmanTreeService
    {
        public static Dictionary<char, string> huffmanDictionary = [];

        static void PrintTree(HuffmanNode? node, string indent)
        {
            if (node == null) return;

            // Exibe o nó atual usando o método ToString()
            Console.WriteLine($"{indent}{node}");

            // Exibe os filhos
            PrintTree(node.Left, indent + "  ");
            PrintTree(node.Right, indent + "  ");
        }

        public static Dictionary<char, string> CreateHuffmanTree(string text)
        {
            // Monta pilha de frequencia
            var orderedFrequency = text
                .GroupBy(c => c)
                .Select(group => new HuffmanNode { Character = group.Key, Frequency = group.Count() })
                .OrderBy(x => x.Frequency) // Ordena pela frequência em ordem crescente
                .ToList();

            // Montagem da árvore
            HuffmanNode root = BuildHuffmanTree(orderedFrequency);
            PrintTree(root, "");

            // Criação do dicionário
            Dictionary<char, string> huffmanCodes = [];
            GenerateCodes(root, "", huffmanCodes);

            huffmanDictionary = huffmanCodes;
            return huffmanCodes;
        }

        static void GenerateCodes(HuffmanNode? node, string currentCode, Dictionary<char, string> codes)
        {
            if (node == null) return;

            // Se for uma folha, associa o caractere ao código atual
            if (node.IsLeaf && node.Character.HasValue)
            {
                codes[node.Character.Value] = currentCode;
            }

            // Vai para a esquerda (adiciona '0' ao código)
            GenerateCodes(node.Left, currentCode + "0", codes);

            // Vai para a direita (adiciona '1' ao código)
            GenerateCodes(node.Right, currentCode + "1", codes);
        }

        public static HuffmanNode BuildHuffmanTree(List<HuffmanNode> nodes)
        {
            // Continua até restar apenas um nó (a raiz)
            while (nodes.Count > 1)
            {
                // Pega os dois nós com menor frequência
                var left = nodes[0];
                var right = nodes[1];

                // Remove os dois nós da lista
                nodes.Remove(left);
                nodes.Remove(right);

                // Cria um novo nó pai com a soma das frequências
                var parent = new HuffmanNode
                {
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };

                // Adiciona o novo nó à lista
                nodes.Add(parent);

                // Reordena a lista pela frequência
                nodes = [.. nodes.OrderBy(x => x.Frequency).ThenBy(x => x.IsLeaf)];
            }

            // O último nó restante é a raiz
            return nodes[0];
        }
    }
}
