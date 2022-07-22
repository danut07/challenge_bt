import { Flex } from "@chakra-ui/react";
//@ts-ignore
export const Content = ({ children }) => {
  return (
    <Flex
      w="inherit"
      h="inherit"
      direction="column"
      justify="center"
      align="center"
    >
      {children}
    </Flex>
  );
};

//@ts-ignore
export const FormView = ({ children }) => {
  return (
    <Flex
      w={["90vw", "90vw", "50vw", "40vw", "30vw"]}
      h="50vh"
      align="left"
      bgColor="blackAlpha.200"
      borderRadius="5%"
      direction="column"
      my="50px"
      px={["4", "4", "4", "8", "8"]}
      py={["4", "4", "4", "8", "8"]}
    >
      {children}
    </Flex>
  );
};
