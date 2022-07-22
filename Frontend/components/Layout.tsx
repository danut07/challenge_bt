import { Flex } from "@chakra-ui/react";
//@ts-ignore
export const Layout = ({ children }) => {
  return (
    <>
      <Flex
        w="100vw"
        h="100vh"
        bgGradient="linear(to-r, yellow.200, purple.600)"
      >
        {children}
      </Flex>
    </>
  );
};
